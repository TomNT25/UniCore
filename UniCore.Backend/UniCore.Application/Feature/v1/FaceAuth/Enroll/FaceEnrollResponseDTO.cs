namespace UniCore.Application.Feature.v1.FaceAuth.Enroll
{
    /// <summary>
    /// Response DTO for face enrollment.
    /// </summary>
    public class FaceEnrollResponseDTO
    {
        /// <summary>
        /// Whether enrollment was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Current face profile status after enrollment.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Whether PIN setup is required to complete enrollment.
        /// True when status is FACE_PENDING_PIN.
        /// </summary>
        public bool RequiresPin { get; set; }

        /// <summary>
        /// Number of images successfully processed.
        /// </summary>
        public int NumImagesUsed { get; set; }

        /// <summary>
        /// Model version used for enrollment.
        /// </summary>
        public string? ModelVersion { get; set; }

        /// <summary>
        /// Error code if enrollment failed.
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Error message if enrollment failed.
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
