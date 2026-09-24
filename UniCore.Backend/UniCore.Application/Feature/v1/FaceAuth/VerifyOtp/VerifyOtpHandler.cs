using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Cache;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Contract.Util;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;
using UniCore.Application.Feature.v1.Auth.Login;
using UniCore.Application.Feature.v1.FaceAuth.Login;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.FaceAuth.VerifyOtp
{
    public class VerifyOtpHandler : IRequestHandler<VerifyOtpRequestDTO, VerifyOtpResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly ICacheService _cacheService;
        private readonly IJwtService _jwtService;
        private readonly IFaceAuthAuditService _auditService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IJsonStringLocalizer _localizer;
        private readonly ILogger<VerifyOtpHandler> _logger;

        private const string ChallengePrefix = "face_challenge:";
        private const int MaxFailedAttempts = 5;

        public VerifyOtpHandler(
            IUserRepository userRepository,
            IUserTokenRepository userTokenRepository,
            ICacheService cacheService,
            IJwtService jwtService,
            IFaceAuthAuditService auditService,
            IMapper mapper,
            IConfiguration configuration,
            IJsonStringLocalizer localizer,
            ILogger<VerifyOtpHandler> logger)
        {
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _cacheService = cacheService;
            _jwtService = jwtService;
            _auditService = auditService;
            _mapper = mapper;
            _configuration = configuration;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<VerifyOtpResponseDTO> HandleAsync(
            VerifyOtpRequestDTO request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing Face Login OTP verification");

            var cacheKey = $"{ChallengePrefix}{request.ChallengeToken}";
            var challengeData = await _cacheService.GetAsync<FaceChallengeData>(cacheKey, cancellationToken);

            if (challengeData == null)
            {
                _logger.LogWarning("Challenge token not found or expired: {Token}",
                    request.ChallengeToken[..Math.Min(10, request.ChallengeToken.Length)]);
                return new VerifyOtpResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InvalidChallenge,
                    ErrorMessage = _localizer.GetString(MessageConstants.FaceAuth.InvalidChallenge)
                };
            }

            if (challengeData.ExpiresAt < DateTime.UtcNow)
            {
                _logger.LogWarning("Face challenge token expired for user {UserId}", challengeData.UserId);
                await _cacheService.RemoveAsync(cacheKey, cancellationToken);
                return new VerifyOtpResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.ChallengeExpired,
                    ErrorMessage = _localizer.GetString(MessageConstants.FaceAuth.ChallengeExpired)
                };
            }

            var userId = challengeData.UserId;

            // Verify OTP
            if (string.IsNullOrWhiteSpace(challengeData.Otp) || !string.Equals(challengeData.Otp.Trim(), request.Otp.Trim(), StringComparison.Ordinal))
            {
                challengeData.FailedOtpAttempts++;
                var remaining = MaxFailedAttempts - challengeData.FailedOtpAttempts;

                _logger.LogWarning("Invalid OTP entered for user {UserId}. Attempts remaining: {Remaining}", userId, remaining);

                if (remaining <= 0)
                {
                    await _cacheService.RemoveAsync(cacheKey, cancellationToken);
                    await _auditService.LogAsync(new FaceAuthAuditEntry
                    {
                        UserId = userId,
                        Action = FaceAuthAction.VerifyOtp,
                        Result = FaceAuthResult.Failed,
                        ErrorCode = FaceAuthConstants.ErrorCodes.PinLocked
                    }, cancellationToken);

                    return new VerifyOtpResponseDTO
                    {
                        Success = false,
                        ErrorCode = FaceAuthConstants.ErrorCodes.PinLocked,
                        ErrorMessage = _localizer.GetString(MessageConstants.FaceAuth.PinLocked),
                        RemainingAttempts = 0
                    };
                }

                // Update attempts in cache
                await _cacheService.SetAsync(cacheKey, challengeData, challengeData.ExpiresAt - DateTime.UtcNow, cancellationToken);

                return new VerifyOtpResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InvalidPin,
                    ErrorMessage = _localizer.GetString(MessageConstants.FaceAuth.InvalidOtp),
                    RemainingAttempts = remaining
                };
            }

            // OTP verified — consume challenge token
            await _cacheService.RemoveAsync(cacheKey, cancellationToken);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("User not found or inactive after OTP verification: {UserId}", userId);
                return new VerifyOtpResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.AccountSuspended,
                    ErrorMessage = "Account is not active."
                };
            }

            // Generate JWT tokens
            var userDto = _mapper.Map<UserDTO>(user);
            var accessToken = _jwtService.GenerateAccessToken(userDto);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpireDays = int.Parse(
                _configuration[AuthConstants.JwtConfig.RefreshTokenExpireDaysPath]
                ?? AuthConstants.JwtConfig.DefaultRefreshTokenExpireDays.ToString());

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

            await _auditService.LogAsync(new FaceAuthAuditEntry
            {
                UserId = userId,
                Action = FaceAuthAction.VerifyOtp,
                Result = FaceAuthResult.Success,
                Similarity = challengeData.Similarity
            }, cancellationToken);

            _logger.LogInformation("Face Login OTP verification successful for user {UserId}", userId);

            return new VerifyOtpResponseDTO
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
