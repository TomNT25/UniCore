namespace UniCore.Application.Feature.v1.FaceAuth.Login
{
    /// <summary>
    /// Response DTO for face login (first step).
    /// Returns challenge token, NOT access token.
    /// </summary>
    public class FaceLoginResponseDTO
    {
        /// <summary>
        /// Whether face recognition was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Short-lived challenge token for PIN verification.
        /// Only present on success. Expires in ~5 minutes.
        /// </summary>
        public string? ChallengeToken { get; set; }

        /// <summary>
        /// When the challenge token expires.
        /// </summary>
        public DateTime? ChallengeExpiresAt { get; set; }

        /// <summary>
        /// Similarity score from face recognition (for debugging).
        /// </summary>
        public double? Similarity { get; set; }

        /// <summary>
        /// Error code if face recognition failed.
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Error message if face recognition failed.
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
