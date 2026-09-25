using FluentValidation;
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
using UniCore.Helper.Constant;
using UserEntity = UniCore.Application.Entity.User;

namespace UniCore.Application.Feature.v1.Auth.VerifyOtp
{
    public class VerifyOtpHandler : IRequestHandler<VerifyOtpRequestDTO, VerifyOtpResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpService _otpService;
        private readonly ICacheService _cacheService;
        private readonly IJwtService _jwtService;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IValidator<VerifyOtpRequestDTO> _validator;
        private readonly ILogger<VerifyOtpHandler> _logger;

        public VerifyOtpHandler(
            IUserRepository userRepository,
            IOtpService otpService,
            ICacheService cacheService,
            IJwtService jwtService,
            IUserTokenRepository userTokenRepository,
            IConfiguration configuration,
            IMapper mapper,
            IValidator<VerifyOtpRequestDTO> validator,
            ILogger<VerifyOtpHandler> logger)
        {
            _userRepository = userRepository;
            _otpService = otpService;
            _cacheService = cacheService;
            _jwtService = jwtService;
            _userTokenRepository = userTokenRepository;
            _configuration = configuration;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        public async Task<VerifyOtpResponseDTO> HandleAsync(VerifyOtpRequestDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // 1. Resolve user by Email or Username
            UserEntity? user = null;
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            }
            if (user == null && !string.IsNullOrWhiteSpace(request.Username))
            {
                user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
            }

            if (user == null)
            {
                throw new KeyNotFoundException(MessageConstants.Auth.UserNotFound);
            }

            // 2. Verify OTP against memory cache
            var otpInput = request.OtpCode.Trim();
            var emailKey = user.Email.Trim().ToLowerInvariant();
            var usernameKey = user.Username.Trim().ToLowerInvariant();

            var isOtpValid = await _otpService.VerifyOtpAsync($"login_otp_{emailKey}", otpInput, cancellationToken)
                          || await _otpService.VerifyOtpAsync($"login_otp_{usernameKey}", otpInput, cancellationToken)
                          || await _otpService.VerifyOtpAsync($"verify_otp_{emailKey}", otpInput, cancellationToken)
                          || await _otpService.VerifyOtpAsync($"verify_otp_{usernameKey}", otpInput, cancellationToken)
                          || await _otpService.VerifyOtpAsync($"verify_email_{emailKey}", otpInput, cancellationToken)
                          || await _otpService.VerifyOtpAsync($"verify_email_{user.Email}", otpInput, cancellationToken);

            if (!isOtpValid)
            {
                _logger.LogWarning("Invalid or expired OTP entered for user {UserId} ({Email})", user.Id, user.Email);
                throw new InvalidOperationException(MessageConstants.Auth.InvalidOrExpiredOtp);
            }

            // Clean up remaining alias keys in cache
            await _cacheService.RemoveAsync($"OTP_login_otp_{emailKey}", cancellationToken);
            await _cacheService.RemoveAsync($"OTP_login_otp_{usernameKey}", cancellationToken);
            await _cacheService.RemoveAsync($"OTP_verify_otp_{emailKey}", cancellationToken);
            await _cacheService.RemoveAsync($"OTP_verify_otp_{usernameKey}", cancellationToken);
            await _cacheService.RemoveAsync($"OTP_verify_email_{emailKey}", cancellationToken);

            // 3. Update user status
            user.IsEmailVerified = true;
            user.EmailVerifiedAt ??= DateTime.UtcNow;
            user.LastLoginAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            // 4. Generate JWT tokens for authenticated session
            var userDto = _mapper.Map<UserDTO>(user);
            var accessToken = _jwtService.GenerateAccessToken(userDto);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpireDays = int.Parse(_configuration[AuthConstants.JwtConfig.RefreshTokenExpireDaysPath] 
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

            _logger.LogInformation("OTP verified and tokens issued successfully for user {UserId} ({Email})", user.Id, user.Email);

            return new VerifyOtpResponseDTO
            {
                Email = user.Email,
                IsVerified = true,
                Message = MessageConstants.Auth.VerifyOtpSuccess,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpire = refreshTokenExpireDays,
                User = _mapper.Map<UserLoginResponseDTO>(userDto)
            };
        }
    }
}
