namespace UniCore.Application.Feature.v1.FaceAuth.GetStatus
{
    /// <summary>
    /// Response DTO for face auth status.
    /// </summary>
    public class GetFaceStatusResponseDTO
    {
        /// <summary>
        /// Face enrollment status: FACE_NOT_ENROLLED, FACE_PENDING_PIN, FACE_ENROLLED, FACE_SUSPENDED
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Whether face is fully enrolled and ready for face login.
        /// </summary>
        public bool IsEnrolled { get; set; }

        /// <summary>
        /// Whether PIN setup is required.
        /// </summary>
        public bool RequiresPin { get; set; }

        /// <summary>
        /// Whether account is suspended.
        /// </summary>
        public bool IsSuspended { get; set; }

        /// <summary>
        /// Whether account is temporarily locked due to failed PIN attempts.
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// When the face was enrolled (if applicable).
        /// </summary>
        public DateTime? EnrolledAt { get; set; }

        /// <summary>
        /// When the PIN was set (if applicable).
        /// </summary>
        public DateTime? PinSetAt { get; set; }

        /// <summary>
        /// When the lockout ends (if locked).
        /// </summary>
        public DateTime? LockoutEnd { get; set; }

        /// <summary>
        /// Face AI model version used for enrollment.
        /// </summary>
        public string? ModelVersion { get; set; }
    }
}
