namespace UniCore.Application.Entity
{
    /// <summary>
    /// Audit log for face authentication actions.
    /// Does NOT store images or embeddings — only metadata.
    /// </summary>
    public class FaceAuthLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Unique request ID for tracing.
        /// </summary>
        public string RequestId { get; set; } = string.Empty;

        /// <summary>
        /// User ID (if known). May be null for failed anonymous login attempts.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Action: ENROLL, SET_PIN, LOGIN, VERIFY_PIN, GET_STATUS
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Result: SUCCESS, FAILED, ERROR
        /// </summary>
        public string Result { get; set; } = string.Empty;

        /// <summary>
        /// Error code if failed (e.g., NO_FACE_DETECTED, INVALID_PIN).
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Face AI model version (if applicable).
        /// </summary>
        public string? ModelVersion { get; set; }

        /// <summary>
        /// Similarity score for recognition (if applicable).
        /// </summary>
        public double? Similarity { get; set; }

        /// <summary>
        /// Request processing latency in milliseconds.
        /// </summary>
        public int? LatencyMs { get; set; }

        /// <summary>
        /// Client IP address.
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// User agent string.
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Additional metadata as JSON (e.g., number of images, stage).
        /// </summary>
        public string? Metadata { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
