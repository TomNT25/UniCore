namespace UniCore.Application.Feature.v1.FaceAuth.SetPin
{
    /// <summary>
    /// Response DTO for setting face auth PIN.
    /// </summary>
    public class SetPinResponseDTO
    {
        /// <summary>
        /// Whether PIN was set successfully.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Current face profile status after setting PIN.
        /// Should be FACE_ENROLLED on success.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Error code if PIN setting failed.
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Error message if PIN setting failed.
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
