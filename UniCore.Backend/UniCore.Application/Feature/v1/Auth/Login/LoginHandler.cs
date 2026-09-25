using System.Security.Cryptography;
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
using UniCore.Helper.Constant;
using UniCore.Helper.ExceptionHandler;

namespace UniCore.Application.Feature.v1.Auth.Login
{
    public class LoginHandler : IRequestHandler<LoginRequestDTO, LoginResponseDTO>
    {
        private readonly GetUserByUsernameQueryHandler _getUserByUsernameQueryHandler;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IConfiguration _configuration;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IUserMfaSettingRepository _userMfaSettingRepository;
        private readonly ICacheService _cacheService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<LoginHandler> _logger;

        public LoginHandler(
            GetUserByUsernameQueryHandler getUserByUsernameQueryHandler,
            IUserRepository userRepository,
            IMapper mapper,
            IJwtService jwtService,
            IPasswordHasherService passwordHasherService,
            IConfiguration configuration,
            IUserTokenRepository userTokenRepository,
            IUserMfaSettingRepository userMfaSettingRepository,
            ICacheService cacheService,
            IEmailSender emailSender,
            ILogger<LoginHandler> logger)
        {
            _getUserByUsernameQueryHandler = getUserByUsernameQueryHandler;
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtService = jwtService;
            _passwordHasherService = passwordHasherService;
            _configuration = configuration;
            _userTokenRepository = userTokenRepository;
            _userMfaSettingRepository = userMfaSettingRepository;
            _cacheService = cacheService;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<LoginResponseDTO> HandleAsync(LoginRequestDTO request, CancellationToken cancellationToken)
        {
            var loginIdentifier = request.Username?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(loginIdentifier))
            {
                throw new UnauthorizedAccessException(MessageConstants.Auth.InvalidCredentials);
            }

            // 1. Resolve user entity from database by Username or Email
            var userEntity = await _userRepository.GetByUsernameAsync(loginIdentifier, cancellationToken);

            if (userEntity == null)
            {
                throw new UnauthorizedAccessException(MessageConstants.Auth.InvalidCredentials);
            }

            // 2. Validate account status
            if (!userEntity.IsActive)
            {
                throw new UnauthorizedAccessException("Account is disabled. Please contact the administrator.");
            }

            if (userEntity.LockoutEnd.HasValue && userEntity.LockoutEnd.Value > DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException($"Account is temporarily locked until {userEntity.LockoutEnd.Value:HH:mm:ss UTC}. Please try again later.");
            }

            // 3. Verify password
            if (string.IsNullOrEmpty(userEntity.PasswordHash))
            {
                throw new UnauthorizedAccessException(MessageConstants.Auth.InvalidCredentials);
            }

            var passwordMatch = _passwordHasherService.VerifyHashedPassword(userEntity.PasswordHash, request.Password);
            if (!passwordMatch)
            {
                userEntity.FailedLoginAttempts++;
                userEntity.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(userEntity, cancellationToken);
                await _userRepository.SaveChangesAsync(cancellationToken);

                throw new UnauthorizedAccessException(MessageConstants.Auth.InvalidCredentials);
            }

            var userDto = _mapper.Map<UserDTO>(userEntity);

            
            if (userEntity.UserRoles.Select(ur => ur.Role).Any(r => r.Name == "Student"))
            {
                // 4. Check MFA settings for user (handles OTP, TOTP, EMAIL; excludes FACE which is handled by Face Login)
                var mfaSettings = await _userMfaSettingRepository.GetByUserIdAsync(userEntity.Id, cancellationToken);
                var mfaSetting = mfaSettings?.FirstOrDefault(s => s != null && s.IsActive && s.IsMfaEnabled && s.MfaMethod != "FACE");

                if (mfaSetting != null)
                {
                    var otpNumber = RandomNumberGenerator.GetInt32(100000, 1000000);
                    var otpCode = otpNumber.ToString("D6");
                    var expiration = TimeSpan.FromMinutes(5);

                    var emailKey = userEntity.Email.Trim().ToLowerInvariant();
                    var usernameKey = userEntity.Username.Trim().ToLowerInvariant();

                    // Store OTP in memory cache under multiple lookup keys
                    await _cacheService.SetStringAsync($"OTP_login_otp_{emailKey}", otpCode, expiration, cancellationToken);
                    await _cacheService.SetStringAsync($"OTP_login_otp_{usernameKey}", otpCode, expiration, cancellationToken);
                    await _cacheService.SetStringAsync($"OTP_verify_otp_{emailKey}", otpCode, expiration, cancellationToken);
                    await _cacheService.SetStringAsync($"OTP_verify_otp_{usernameKey}", otpCode, expiration, cancellationToken);
                    await _cacheService.SetStringAsync($"OTP_verify_email_{emailKey}", otpCode, expiration, cancellationToken);

                    _logger.LogInformation("MFA OTP generated for user {UserId} ({Email})", userEntity.Id, userEntity.Email);

                    // Send email with OTP to user
                    try
                    {
                        var emailSubject = "UniCore - Mã OTP xác thực đăng nhập";
                        var emailBody = $@"
                        <!DOCTYPE html>
                        <html lang='en'>
                        <head>
                            <meta charset='UTF-8' />
                            <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                            <title>UniCore Login Verification</title>
                        </head>

                        <body style='margin: 0; padding: 0; background-color: #f8fafc; font-family: Arial, Helvetica, sans-serif; color: #1e293b;'>

                            <div style='padding: 40px 20px;'>
                                <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; border: 1px solid #e2e8f0; border-radius: 12px; overflow: hidden;'>

                                    <!-- Header -->
                                    <div style='background-color: #2563eb; padding: 28px 30px; text-align: center;'>
                                        <h1 style='margin: 0; color: #ffffff; font-size: 24px; font-weight: 700;'>
                                            UniCore
                                        </h1>
                                        <p style='margin: 8px 0 0; color: #dbeafe; font-size: 14px;'>
                                            Account Security
                                        </p>
                                    </div>

                                    <!-- Content -->
                                    <div style='padding: 35px 30px;'>

                                        <h2 style='margin: 0 0 20px; color: #0f172a; font-size: 22px;'>
                                            Verify Your Login
                                        </h2>

                                        <p style='margin: 0 0 16px; font-size: 15px; line-height: 1.6;'>
                                            Dear <strong>{userEntity.Username}</strong>,
                                        </p>

                                        <p style='margin: 0 0 16px; font-size: 15px; line-height: 1.6; color: #475569;'>
                                            We received a request to sign in to your UniCore account.
                                            To complete the login process, please enter the verification code below:
                                        </p>

                                        <!-- OTP -->
                                        <div style='margin: 28px 0; padding: 22px; background-color: #f1f5f9; border: 1px solid #e2e8f0; border-radius: 10px; text-align: center;'>
                                            <p style='margin: 0 0 10px; color: #64748b; font-size: 12px; text-transform: uppercase; letter-spacing: 1px;'>
                                                Verification Code
                                            </p>

                                            <div style='font-size: 32px; font-weight: 700; letter-spacing: 8px; color: #2563eb;'>
                                                {otpCode}
                                            </div>
                                        </div>

                                        <!-- Expiration Notice -->
                                        <div style='padding: 14px 16px; background-color: #eff6ff; border-left: 4px solid #2563eb; border-radius: 4px;'>
                                            <p style='margin: 0; font-size: 13px; line-height: 1.5; color: #1e40af;'>
                                                <strong>This code expires in 5 minutes.</strong>
                                                Please do not share this code with anyone.
                                            </p>
                                        </div>

                                        <p style='margin: 25px 0 0; font-size: 14px; line-height: 1.6; color: #64748b;'>
                                            If you did not attempt to sign in to your account, please change your
                                            password immediately and contact your system administrator if necessary.
                                        </p>

                                    </div>

                                    <!-- Footer -->
                                    <div style='padding: 20px 30px; background-color: #f8fafc; border-top: 1px solid #e2e8f0; text-align: center;'>
                                        <p style='margin: 0; color: #94a3b8; font-size: 12px; line-height: 1.5;'>
                                            This is an automated message from UniCore.<br />
                                            Please do not reply to this email.
                                        </p>
                                    </div>

                                </div>

                                <p style='margin: 20px 0 0; text-align: center; color: #94a3b8; font-size: 11px;'>
                                    © {DateTime.UtcNow.Year} UniCore. All rights reserved.
                                </p>
                            </div>

                        </body>
                        </html>";

                        await _emailSender.SendAsync(userEntity.Email, emailSubject, emailBody, cancellationToken);
                        _logger.LogInformation("MFA OTP email dispatched successfully to {Email}", userEntity.Email);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send MFA OTP email to {Email}", userEntity.Email);
                        throw new BadGatewayException("Failed to send OTP verification email. Please try again later.");
                    }

                    return new LoginResponseDTO
                    {
                        RequiresMfa = true,
                        Email = userEntity.Email,
                        MaskedEmail = MaskEmail(userEntity.Email),
                        Message = "MFA is enabled. An OTP code has been sent to your email. Please verify OTP to complete login.",
                        User = _mapper.Map<UserLoginResponseDTO>(userDto)
                    };
                }
            }

            // 5. Standard login without MFA -> Update last login time & reset failed attempts
            userEntity.LastLoginAt = DateTime.UtcNow;
            userEntity.FailedLoginAttempts = 0;
            userEntity.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(userEntity, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            // 6. Generate tokens
            var accessToken = _jwtService.GenerateAccessToken(userDto);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenConfig = _configuration[AuthConstants.JwtConfig.RefreshTokenExpireDaysPath];
            var refreshTokenExpireDays = int.TryParse(refreshTokenConfig, out var parsedDays) && parsedDays > 0
                ? parsedDays
                : AuthConstants.JwtConfig.DefaultRefreshTokenExpireDays;

            var userToken = new UserToken
            {
                UserId = userEntity.Id,
                RefreshToken = refreshToken,
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpireDays),
                IsActive = true
            };
            await _userTokenRepository.AddAsync(userToken, cancellationToken);
            await _userTokenRepository.SaveChangesAsync(cancellationToken);

            return new LoginResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpire = refreshTokenExpireDays,
                User = _mapper.Map<UserLoginResponseDTO>(userDto)
            };
        }

        private static string MaskEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                return email;

            var parts = email.Split('@');
            var name = parts[0];
            var domain = parts[1];

            if (name.Length <= 2)
                return $"{name[0]}***@{domain}";

            return $"{name[0]}***{name[^1]}@{domain}";
        }
    }
}
