using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UniCore.Application.Contract.Cache;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Helper.Constant;

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
        private readonly ILogger<FaceLoginHandler> _logger;

        // Challenge token config
        private const string ChallengePrefix = "face_challenge:";
        private const int DefaultChallengeMinutes = 5;

        public FaceLoginHandler(
            IFaceAiClient faceAiClient,
            IUserFaceProfileRepository faceProfileRepository,
            IUserRepository userRepository,
            ICacheService cacheService,
            IFaceAuthAuditService auditService,
            IConfiguration configuration,
            ILogger<FaceLoginHandler> logger)
        {
            _faceAiClient = faceAiClient;
            _faceProfileRepository = faceProfileRepository;
            _userRepository = userRepository;
            _cacheService = cacheService;
            _auditService = auditService;
            _configuration = configuration;
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

            // Generate challenge token
            var challengeToken = GenerateChallengeToken();
            var challengeMinutes = GetChallengeMinutes();
            var expiresAt = DateTime.UtcNow.AddMinutes(challengeMinutes);

            // Store challenge in cache
            var cacheKey = $"{ChallengePrefix}{challengeToken}";
            var challengeData = new FaceChallengeData
            {
                UserId = userId,
                Similarity = recognizeResult.Similarity,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt
            };

            await _cacheService.SetAsync(
                cacheKey,
                challengeData,
                TimeSpan.FromMinutes(challengeMinutes + 1), // Extra minute for safety
                cancellationToken);

            _logger.LogInformation(
                "Face login challenge issued for user {UserId}. Similarity: {Similarity}, ExpiresAt: {ExpiresAt}",
                userId, recognizeResult.Similarity, expiresAt);

            stopwatch.Stop();

            // Audit log success
            await _auditService.LogAsync(new FaceAuthAuditEntry
            {
                RequestId = requestId,
                UserId = userId,
                Action = FaceAuthAction.Login,
                Result = FaceAuthResult.Success,
                ModelVersion = recognizeResult.ModelVersion,
                Similarity = recognizeResult.Similarity,
                LatencyMs = (int)stopwatch.ElapsedMilliseconds,
                IpAddress = request.IpAddress,
                UserAgent = request.UserAgent
            }, cancellationToken);

            return new FaceLoginResponseDTO
            {
                Success = true,
                ChallengeToken = challengeToken,
                ChallengeExpiresAt = expiresAt,
                Similarity = recognizeResult.Similarity
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
        public double Similarity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
