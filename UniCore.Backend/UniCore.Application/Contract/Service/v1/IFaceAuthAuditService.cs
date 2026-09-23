using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Service.v1
{
    /// <summary>
    /// Service for face authentication audit logging and rate limiting.
    /// </summary>
    public interface IFaceAuthAuditService
    {
        /// <summary>
        /// Log a face authentication action.
        /// </summary>
        Task LogAsync(FaceAuthAuditEntry entry, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if IP is rate limited for login attempts.
        /// </summary>
        Task<RateLimitResult> CheckIpRateLimitAsync(
            string ipAddress,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if user is rate limited for login attempts.
        /// </summary>
        Task<RateLimitResult> CheckUserRateLimitAsync(
            string userId,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Entry for audit logging.
    /// </summary>
    public class FaceAuthAuditEntry
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        public FaceAuthAction Action { get; set; }
        public FaceAuthResult Result { get; set; }
        public string? ErrorCode { get; set; }
        public string? ModelVersion { get; set; }
        public double? Similarity { get; set; }
        public int? LatencyMs { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }
    }

    public enum FaceAuthAction
    {
        Enroll,
        SetPin,
        Login,
        VerifyPin,
        GetStatus
    }

    public enum FaceAuthResult
    {
        Success,
        Failed,
        Error
    }

    /// <summary>
    /// Result of rate limit check.
    /// </summary>
    public class RateLimitResult
    {
        public bool IsLimited { get; set; }
        public int CurrentCount { get; set; }
        public int MaxAllowed { get; set; }
        public TimeSpan? RetryAfter { get; set; }
        public string? Message { get; set; }
    }
}
