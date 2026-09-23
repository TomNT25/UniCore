using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Cache;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Util;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Auth.Login;
using UniCore.Application.Feature.v1.FaceAuth.Login;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.FaceAuth.VerifyPin
{
    public class VerifyPinHandler : IRequestHandler<VerifyPinRequestDTO, VerifyPinResponseDTO>
    {
        private readonly IUserFaceProfileRepository _faceProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly ICacheService _cacheService;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<VerifyPinHandler> _logger;

        private const string ChallengePrefix = "face_challenge:";

        public VerifyPinHandler(
            IUserFaceProfileRepository faceProfileRepository,
            IUserRepository userRepository,
            IUserTokenRepository userTokenRepository,
            ICacheService cacheService,
            IPasswordHasherService passwordHasher,
            IJwtService jwtService,
            IMapper mapper,
            IConfiguration configuration,
            ILogger<VerifyPinHandler> logger)
        {
            _faceProfileRepository = faceProfileRepository;
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _cacheService = cacheService;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<VerifyPinResponseDTO> HandleAsync(
            VerifyPinRequestDTO request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing face login PIN verification");

            // Retrieve challenge from cache
            var cacheKey = $"{ChallengePrefix}{request.ChallengeToken}";
            var challengeData = await _cacheService.GetAsync<FaceChallengeData>(cacheKey, cancellationToken);

            if (challengeData == null)
            {
                _logger.LogWarning("Challenge token not found or expired: {Token}", request.ChallengeToken[..Math.Min(10, request.ChallengeToken.Length)]);
                return new VerifyPinResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InvalidPin, // Don't leak challenge status
                    ErrorMessage = "Invalid or expired challenge. Please try face login again."
                };
            }

            // Check if challenge has expired
            if (challengeData.ExpiresAt < DateTime.UtcNow)
            {
                _logger.LogWarning("Challenge token expired: {UserId}", challengeData.UserId);
                await _cacheService.RemoveAsync(cacheKey, cancellationToken);
                return new VerifyPinResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InvalidPin,
                    ErrorMessage = "Challenge has expired. Please try face login again."
                };
            }

            var userId = challengeData.UserId;

            // Get face profile
            var faceProfile = await _faceProfileRepository.GetByUserIdAsync(userId, cancellationToken);
            if (faceProfile == null || faceProfile.Status != FaceAuthConstants.Status.Enrolled)
            {
                _logger.LogWarning("Face profile not found or not enrolled: {UserId}", userId);
                await _cacheService.RemoveAsync(cacheKey, cancellationToken);
                return new VerifyPinResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.NotEnrolled,
                    ErrorMessage = "Face authentication is not set up"
                };
            }

            // Check if PIN is locked
            if (faceProfile.PinLockoutEnd.HasValue && faceProfile.PinLockoutEnd > DateTime.UtcNow)
            {
                _logger.LogWarning("PIN is locked: {UserId}, LockoutEnd: {LockoutEnd}", userId, faceProfile.PinLockoutEnd);
                return new VerifyPinResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.PinLocked,
                    ErrorMessage = $"Too many failed attempts. Try again after {faceProfile.PinLockoutEnd:HH:mm:ss}"
                };
            }

            // Verify PIN
            if (string.IsNullOrWhiteSpace(faceProfile.PinHash) ||
                !_passwordHasher.VerifyHashedPassword(faceProfile.PinHash, request.Pin))
            {
                _logger.LogWarning("Invalid PIN for user: {UserId}", userId);

                // Increment failed attempts
                var updatedProfile = await _faceProfileRepository.IncrementFailedPinAttemptsAsync(userId, cancellationToken);
                var remaining = FaceAuthConstants.Pin.MaxFailedAttempts - updatedProfile.FailedPinAttempts;

                if (remaining <= 0)
                {
                    // Consume the challenge on lockout
                    await _cacheService.RemoveAsync(cacheKey, cancellationToken);

                    return new VerifyPinResponseDTO
                    {
                        Success = false,
                        ErrorCode = FaceAuthConstants.ErrorCodes.PinLocked,
                        ErrorMessage = $"Too many failed attempts. Please try again after {FaceAuthConstants.Pin.LockoutMinutes} minutes.",
                        RemainingAttempts = 0
                    };
                }

                return new VerifyPinResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InvalidPin,
                    ErrorMessage = $"Invalid PIN. {remaining} attempts remaining.",
                    RemainingAttempts = remaining
                };
            }

            // PIN is correct — reset failed attempts
            await _faceProfileRepository.ResetFailedPinAttemptsAsync(userId, cancellationToken);

            // Consume the challenge (one-time use)
            await _cacheService.RemoveAsync(cacheKey, cancellationToken);

            // Get user for JWT generation
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("User not found or inactive after PIN verification: {UserId}", userId);
                return new VerifyPinResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.AccountSuspended,
                    ErrorMessage = "Account is not active"
                };
            }

            // Generate tokens (same as LoginHandler)
            var userDto = _mapper.Map<UserDTO>(user);
            var accessToken = _jwtService.GenerateAccessToken(userDto);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpireDays = int.Parse(
                _configuration[AuthConstants.JwtConfig.RefreshTokenExpireDaysPath]
                ?? AuthConstants.JwtConfig.DefaultRefreshTokenExpireDays.ToString());

            // Save refresh token
            var userToken = new UserToken
            {
                UserId = user.Id,
                RefreshToken = refreshToken,
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpireDays),
                IsActive = true
            };
            await _userTokenRepository.AddAsync(userToken, cancellationToken);
            await _userTokenRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Face login successful for user {UserId}. Similarity: {Similarity}",
                userId, challengeData.Similarity);

            return new VerifyPinResponseDTO
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpire = refreshTokenExpireDays,
                User = _mapper.Map<UserLoginResponseDTO>(userDto)
            };
        }
    }
}
