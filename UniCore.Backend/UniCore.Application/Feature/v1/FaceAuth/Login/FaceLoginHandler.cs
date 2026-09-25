using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Cache;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.Application.Feature.v1.FaceAuth.Login
{
    public class FaceLoginHandler : IRequestHandler<FaceLoginRequestDTO, FaceLoginResponseDTO>
    {
        private readonly IFaceAiClient _faceAiClient;
        private readonly IUserFaceProfileRepository _faceProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;
        private readonly IFaceAuthAuditService _auditService;
        private readonly IConfiguration _configuration;
        private readonly IUserMfaSettingRepository _mfaSettingRepository;
        private readonly IEmailSender _emailSender;
        private readonly IJsonStringLocalizer _localizer;
        private readonly ILogger<FaceLoginHandler> _logger;

        // Challenge token config
        private const string ChallengePrefix = "face_challenge:";
        private const int DefaultChallengeMinutes = 5;

        public FaceLoginHandler(
            IFaceAiClient faceAiClient,
            IUserFaceProfileRepository faceProfileRepository,
            IUserRepository userRepository,
            IUserMfaSettingRepository mfaSettingRepository,
            IEmailSender emailSender,
            ICacheService cacheService,
            IFaceAuthAuditService auditService,
            IConfiguration configuration,
            IJsonStringLocalizer localizer,
            ILogger<FaceLoginHandler> logger)
        {
            _faceAiClient = faceAiClient;
            _faceProfileRepository = faceProfileRepository;
            _userRepository = userRepository;
            _mfaSettingRepository = mfaSettingRepository;
            _emailSender = emailSender;
            _cacheService = cacheService;
            _auditService = auditService;
            _configuration = configuration;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<FaceLoginResponseDTO> HandleAsync(
            FaceLoginRequestDTO request,
            CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString();
            _logger.LogInformation("Processing face login request {RequestId}", requestId);

            // Check IP rate limit
            if (!string.IsNullOrWhiteSpace(request.IpAddress))
            {
                var ipRateLimit = await _auditService.CheckIpRateLimitAsync(request.IpAddress, cancellationToken);
                if (ipRateLimit.IsLimited)
                {
                    stopwatch.Stop();
                    await LogFailureAsync(requestId, null, FaceAuthConstants.ErrorCodes.RateLimited,
                        (int)stopwatch.ElapsedMilliseconds, request, cancellationToken);

                    return new FaceLoginResponseDTO
                    {
                        Success = false,
                        ErrorCode = FaceAuthConstants.ErrorCodes.RateLimited,
                        ErrorMessage = ipRateLimit.Message ?? "Too many login attempts. Please try again later."
                    };
                }
            }

            // Call Face AI recognize
            var recognizeResult = await _faceAiClient.RecognizeAsync(
                new FaceImageInput
                {
                    Stream = request.FaceStream,
                    FileName = request.FileName,
                    ContentType = request.ContentType,
                    Length = request.Length
                },
                cancellationToken);

            if (!recognizeResult.Success)
            {
                _logger.LogWarning(
                    "Face recognition failed: {ErrorCode} - {ErrorMessage}",
                    recognizeResult.ErrorCode, recognizeResult.ErrorMessage);

                // Map NO_CANDIDATES to NO_MATCH for security (don't leak existence)
                var errorCode = recognizeResult.ErrorCode == FaceAuthConstants.ErrorCodes.NoCandidates
                    ? FaceAuthConstants.ErrorCodes.NoMatch
                    : recognizeResult.ErrorCode;

                return new FaceLoginResponseDTO
                {
                    Success = false,
                    ErrorCode = errorCode,
                    ErrorMessage = GetFriendlyErrorMessage(errorCode)
                };
            }

            var userId = recognizeResult.UserId;
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("Face AI returned success but no user_id");
                return new FaceLoginResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.NoMatch,
                    ErrorMessage = "No matching user found"
                };
            }

            // Verify user exists and is active
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning(
                    "Face matched but user not found or inactive: {UserId}",
                    userId);
                return new FaceLoginResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.NoMatch,
                    ErrorMessage = "No matching user found"
                };
            }

            // Verify face profile exists and is enrolled
            var faceProfile = await _faceProfileRepository.GetByUserIdAsync(userId, cancellationToken);
            if (faceProfile == null)
            {
                _logger.LogWarning(
                    "Face matched but no face profile found: {UserId}",
                    userId);
                return new FaceLoginResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.NoMatch,
                    ErrorMessage = "No matching user found"
                };
            }

            if (faceProfile.Status == FaceAuthConstants.Status.Suspended)
            {
                _logger.LogWarning(
                    "Face matched but profile is suspended: {UserId}",
                    userId);
                return new FaceLoginResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.AccountSuspended,
                    ErrorMessage = "Face authentication is suspended for this account"
                };
            }

            if (faceProfile.Status != FaceAuthConstants.Status.Enrolled)
            {
                _logger.LogWarning(
                    "Face matched but profile is not enrolled: {UserId}, Status: {Status}",
                    userId, faceProfile.Status);
                return new FaceLoginResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.NotEnrolled,
                    ErrorMessage = "Face authentication is not fully set up"
                };
            }

            // Verify user has enabled Face Recognition MFA
            var mfaSettingList = await _mfaSettingRepository.GetByUserIdAsync(userId, cancellationToken);
            var mfaSetting = mfaSettingList.FirstOrDefault(s => s != null && s.IsActive && s.MfaMethod == "FACE");
            if (mfaSetting == null || !mfaSetting.IsMfaEnabled)
            {
                _logger.LogWarning("Face matched but Face Recognition MFA is not enabled for user: {UserId}", userId);
                return new FaceLoginResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.NotEnrolled,
                    ErrorMessage = _localizer.GetString(MessageConstants.FaceAuth.MfaNotEnabled)
                };
            }

            // Check if PIN is locked
            if (faceProfile.PinLockoutEnd.HasValue && faceProfile.PinLockoutEnd > DateTime.UtcNow)
            {
                _logger.LogWarning(
                    "Face matched but PIN is locked: {UserId}, LockoutEnd: {LockoutEnd}",
                    userId, faceProfile.PinLockoutEnd);
                return new FaceLoginResponseDTO
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.PinLocked,
                    ErrorMessage = $"Too many failed PIN attempts. Try again after {faceProfile.PinLockoutEnd:HH:mm:ss}"
                };
            }

            // Generate secure 6-digit numeric OTP
            var otp = System.Security.Cryptography.RandomNumberGenerator.GetInt32(100000, 1000000).ToString("D6");

            // Generate challenge token
            var challengeToken = GenerateChallengeToken();
            var challengeMinutes = GetChallengeMinutes();
            var expiresAt = DateTime.UtcNow.AddMinutes(challengeMinutes);

            // Store challenge in cache
            var cacheKey = $"{ChallengePrefix}{challengeToken}";
            var challengeData = new FaceChallengeData
            {
                UserId = userId,
                Email = user.Email,
                Otp = otp,
                FailedOtpAttempts = 0,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt
            };

            await _cacheService.SetAsync(
                cacheKey,
                challengeData,
                TimeSpan.FromMinutes(challengeMinutes + 1), // Extra minute for safety
                cancellationToken);

            _logger.LogInformation(
                "Face login challenge issued for user {UserId}, ExpiresAt: {ExpiresAt}",
                userId, expiresAt);

            // Dispatch OTP via Email (StimulationEmailProvider / Smtp)
            try
            {
                var emailSubject = "UniCore - Mã OTP xác thực đăng nhập khuôn mặt";
                var emailBody = $@"
<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e2e8f0; border-radius: 8px;'>
    <h2 style='color: #2563eb;'>Xác thực đăng nhập sinh trắc học</h2>
    <p>Xin chào <strong>{user.Username}</strong>,</p>
    <p>Hệ thống vừa nhận diện thành công khuôn mặt của bạn để đăng nhập vào UniCore.</p>
    <p>Mã OTP xác thực đăng nhập của bạn là:</p>
    <div style='background-color: #f1f5f9; padding: 15px; border-radius: 8px; font-size: 28px; font-weight: bold; letter-spacing: 5px; text-align: center; color: #1e293b; margin: 20px 0;'>
        {otp}
    </div>
    <p>Mã OTP có hiệu lực trong vòng <strong>{challengeMinutes} phút</strong>. Vui lòng không chia sẻ mã này cho bất kỳ ai.</p>
    <hr style='border: none; border-top: 1px solid #e2e8f0; margin: 20px 0;' />
    <p style='color: #64748b; font-size: 12px;'>Nếu bạn không thực hiện đăng nhập này, vui lòng liên hệ quản trị viên ngay lập tức.</p>
</div>";

                await _emailSender.SendAsync(user.Email, emailSubject, emailBody, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send Face Login OTP email to {Email}", user.Email);
            }

            stopwatch.Stop();

            // Audit log success
            await _auditService.LogAsync(new FaceAuthAuditEntry
            {
                RequestId = requestId,
                UserId = userId,
                Action = FaceAuthAction.Login,
                Result = FaceAuthResult.Success,
                LatencyMs = (int)stopwatch.ElapsedMilliseconds,
                IpAddress = request.IpAddress,
                UserAgent = request.UserAgent
            }, cancellationToken);

            return new FaceLoginResponseDTO
            {
                Success = true,
                ChallengeToken = challengeToken,
                ChallengeExpiresAt = expiresAt,
                MaskedEmail = MaskEmail(user.Email),
                Message = _localizer.GetString(MessageConstants.FaceAuth.LoginSuccess)
            };
        }

        private string GenerateChallengeToken()
        {
            // Generate a secure random token
            var bytes = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

        private int GetChallengeMinutes()
        {
            var configValue = _configuration["FaceAi:ChallengeMinutes"];
            return int.TryParse(configValue, out var minutes) && minutes > 0
                ? minutes
                : DefaultChallengeMinutes;
        }

        private static string GetFriendlyErrorMessage(string? errorCode)
        {
            return errorCode switch
            {
                FaceAuthConstants.ErrorCodes.NoFaceDetected => "No face detected in image",
                FaceAuthConstants.ErrorCodes.MultipleFaces => "Multiple faces detected. Please ensure only one face is in the image",
                FaceAuthConstants.ErrorCodes.SpoofDetected => "Liveness check failed. Please use a real face",
                FaceAuthConstants.ErrorCodes.ImageTooBlurry => "Image is too blurry. Please capture a clearer image",
                FaceAuthConstants.ErrorCodes.NoMatch or FaceAuthConstants.ErrorCodes.NoCandidates =>
                    "No matching face found. Please try again or use password login",
                FaceAuthConstants.ErrorCodes.RateLimited => "Too many login attempts. Please try again later.",
                _ => "Face recognition failed. Please try again"
            };
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

        private async Task LogFailureAsync(
            string requestId,
            string? userId,
            string errorCode,
            int latencyMs,
            FaceLoginRequestDTO request,
            CancellationToken cancellationToken)
        {
            await _auditService.LogAsync(new FaceAuthAuditEntry
            {
                RequestId = requestId,
                UserId = userId,
                Action = FaceAuthAction.Login,
                Result = FaceAuthResult.Failed,
                ErrorCode = errorCode,
                LatencyMs = latencyMs,
                IpAddress = request.IpAddress,
                UserAgent = request.UserAgent
            }, cancellationToken);
        }
    }

    /// <summary>
    /// Data stored in cache for face challenge validation.
    /// </summary>
    public class FaceChallengeData
    {
        public string UserId { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Otp { get; set; }
        public int FailedOtpAttempts { get; set; } = 0;
        public double Similarity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
