namespace UniCore.Application.Entity
{
    /// <summary>
    /// Face biometric profile for user face authentication.
    /// Stores enrollment status, PIN hash, and reference to AI embedding.
    /// </summary>
    public class UserFaceProfile
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// FACE_NOT_ENROLLED | FACE_PENDING_PIN | FACE_ENROLLED | FACE_SUSPENDED
        /// </summary>
        public string Status { get; set; } = "FACE_NOT_ENROLLED";

        /// <summary>
        /// Opaque ID returned by Face AI /enroll endpoint.
        /// Stored for audit/reference only; BE does not perform vector search.
        /// </summary>
        public string? EmbeddingId { get; set; }

        /// <summary>
        /// Face AI model version used for enrollment (e.g., "buffalo_l_v1").
        /// </summary>
        public string? ModelVersion { get; set; }

        /// <summary>
        /// BCrypt hash of 6-digit PIN (salted). Never store plaintext.
        /// </summary>
        public string? PinHash { get; set; }

        /// <summary>
        /// Number of consecutive failed PIN attempts. Reset on success.
        /// </summary>
        public int FailedPinAttempts { get; set; } = 0;

        /// <summary>
        /// If set, account is locked until this time.
        /// </summary>
        public DateTime? PinLockoutEnd { get; set; }

        public DateTime? EnrolledAt { get; set; }
        public DateTime? PinSetAt { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
