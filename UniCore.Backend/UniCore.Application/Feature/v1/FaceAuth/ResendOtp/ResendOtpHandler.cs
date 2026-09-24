using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Cache;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.FaceAuth.Login;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.FaceAuth.ResendOtp
{
    public class ResendOtpHandler : IRequestHandler<ResendOtpRequestDTO, ResendOtpResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;
        private readonly IEmailSender _emailSender;
        private readonly IFaceAuthAuditService _auditService;
        private readonly IConfiguration _configuration;
        private readonly IValidator<ResendOtpRequestDTO> _validator;
        private readonly IJsonStringLocalizer _localizer;
        private readonly ILogger<ResendOtpHandler> _logger;

        private const string ChallengePrefix = "face_challenge:";
        private const int DefaultChallengeMinutes = 5;

        public ResendOtpHandler(
            IUserRepository userRepository,
            ICacheService cacheService,
            IEmailSender emailSender,
            IFaceAuthAuditService auditService,
            IConfiguration configuration,
            IValidator<ResendOtpRequestDTO> validator,
            IJsonStringLocalizer localizer,
            ILogger<ResendOtpHandler> logger)
        {
            _userRepository = userRepository;
            _cacheService = cacheService;
            _emailSender = emailSender;
            _auditService = auditService;
            _configuration = configuration;
            _validator = validator;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<ResendOtpResponseDTO> HandleAsync(
            ResendOtpRequestDTO request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var cacheKey = $"{ChallengePrefix}{request.ChallengeToken}";
            var challengeData = await _cacheService.GetAsync<FaceChallengeData>(cacheKey, cancellationToken);

            if (challengeData == null)
            {
                _logger.LogWarning("Challenge token not found or expired for resend: {Token}",
                    request.ChallengeToken[..Math.Min(10, request.ChallengeToken.Length)]);
                return new ResendOtpResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InvalidChallenge,
                    ErrorMessage = _localizer.GetString(MessageConstants.FaceAuth.InvalidChallenge)
                };
            }

            var user = await _userRepository.GetByIdAsync(challengeData.UserId, cancellationToken);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("User not found or inactive for OTP resend: {UserId}", challengeData.UserId);
                return new ResendOtpResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.AccountSuspended,
                    ErrorMessage = "User account not found or inactive."
                };
            }

            // Generate new 6-digit OTP
            var otp = System.Security.Cryptography.RandomNumberGenerator.GetInt32(100000, 1000000).ToString("D6");
            var challengeMinutes = GetChallengeMinutes();
            var expiresAt = DateTime.UtcNow.AddMinutes(challengeMinutes);

            challengeData.Otp = otp;
            challengeData.FailedOtpAttempts = 0;
            challengeData.ExpiresAt = expiresAt;

            await _cacheService.SetAsync(
                cacheKey,
                challengeData,
                TimeSpan.FromMinutes(challengeMinutes + 1),
                cancellationToken);

            // Send OTP email
            try
            {
                var emailSubject = "UniCore - Mã OTP mới xác thực đăng nhập khuôn mặt";
                var emailBody = $@"
<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e2e8f0; border-radius: 8px;'>
    <h2 style='color: #2563eb;'>Xác thực đăng nhập sinh trắc học</h2>
    <p>Xin chào <strong>{user.Username}</strong>,</p>
    <p>Bạn vừa yêu cầu gửi lại mã OTP đăng nhập bằng khuôn mặt vào UniCore.</p>
    <p>Mã OTP xác thực mới của bạn là:</p>
    <div style='background-color: #f1f5f9; padding: 15px; border-radius: 8px; font-size: 28px; font-weight: bold; letter-spacing: 5px; text-align: center; color: #1e293b; margin: 20px 0;'>
        {otp}
    </div>
    <p>Mã OTP có hiệu lực trong vòng <strong>{challengeMinutes} phút</strong>. Vui lòng không chia sẻ mã này cho bất kỳ ai.</p>
    <hr style='border: none; border-top: 1px solid #e2e8f0; margin: 20px 0;' />
    <p style='color: #64748b; font-size: 12px;'>Nếu bạn không thực hiện yêu cầu này, vui lòng liên hệ quản trị viên ngay lập tức.</p>
</div>";

                await _emailSender.SendAsync(user.Email, emailSubject, emailBody, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to resend Face Login OTP email to {Email}", user.Email);
            }

            await _auditService.LogAsync(new FaceAuthAuditEntry
            {
                UserId = user.Id,
                Action = FaceAuthAction.ResendOtp,
                Result = FaceAuthResult.Success
            }, cancellationToken);

            _logger.LogInformation("New OTP generated and resent for user {UserId}", user.Id);

            return new ResendOtpResponseDTO
            {
                Success = true,
                MaskedEmail = MaskEmail(user.Email),
                Message = _localizer.GetString(MessageConstants.FaceAuth.OtpSentSuccess)
            };
        }

        private int GetChallengeMinutes()
        {
            var configValue = _configuration["FaceAi:ChallengeMinutes"];
            return int.TryParse(configValue, out var minutes) && minutes > 0
                ? minutes
                : DefaultChallengeMinutes;
        }

        private static string MaskEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return string.Empty;
            var atIndex = email.IndexOf('@');
            if (atIndex <= 1) return email;
            var localPart = email[..atIndex];
            var domain = email[atIndex..];
            var maskedLocal = localPart.Length <= 2
                ? localPart[0] + "***"
                : localPart[0] + new string('*', Math.Min(localPart.Length - 2, 4)) + localPart[^1];
            return maskedLocal + domain;
        }
    }
}
