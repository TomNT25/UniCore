namespace UniCore.Application.Contract.Service.v1
{
    /// <summary>
    /// HTTP client for Face Recognition AI service.
    /// </summary>
    public interface IFaceAiClient
    {
        /// <summary>
        /// Enroll face images for a user with multi-angle support (up to 5 images).
        /// </summary>
        /// <param name="userId">User ID to associate with the face enrollment.</param>
        /// <param name="username">Display name for the user.</param>
        /// <param name="faceImages">List of face images (1-5 images).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Enrollment result with embedding_id on success.</returns>
        Task<FaceEnrollResult> EnrollAsync(
            string userId,
            string username,
            IReadOnlyList<FaceImageInput> faceImages,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Recognize a face from a single image.
        /// </summary>
        /// <param name="faceImage">Face image to recognize.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Recognition result with matched user_id and similarity on success.</returns>
        Task<FaceRecognizeResult> RecognizeAsync(
            FaceImageInput faceImage,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Input model for face image upload.
    /// </summary>
    public class FaceImageInput
    {
        public Stream Stream { get; set; } = Stream.Null;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = "image/jpeg";
        public long Length { get; set; }
    }

    /// <summary>
    /// Result from face enrollment API.
    /// </summary>
    public class FaceEnrollResult
    {
        public bool Success { get; set; }
        public string? RequestId { get; set; }
        public string? ModelVersion { get; set; }
        public string? EmbeddingId { get; set; }
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public int NumImagesUsed { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Stage { get; set; }
    }

    /// <summary>
    /// Result from face recognition API.
    /// </summary>
    public class FaceRecognizeResult
    {
        public bool Success { get; set; }
        public string? RequestId { get; set; }
        public string? ModelVersion { get; set; }
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public double Similarity { get; set; }
        public double Threshold { get; set; }
        public string? Status { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Stage { get; set; }
    }
}
