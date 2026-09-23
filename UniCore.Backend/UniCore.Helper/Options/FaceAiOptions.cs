namespace UniCore.Helper.Options
{
    /// <summary>
    /// Configuration options for Face AI HTTP client.
    /// </summary>
    public class FaceAiOptions
    {
        public const string SectionName = "FaceAi";

        /// <summary>
        /// Base URL of Face Recognition FastAPI, e.g., http://127.0.0.1:8001
        /// </summary>
        public string BaseUrl { get; set; } = "http://127.0.0.1:8001";

        /// <summary>
        /// POST /ai/face/enroll — multipart: user_id, username, face_1...face_5
        /// </summary>
        public string EnrollPath { get; set; } = "/ai/face/enroll";

        /// <summary>
        /// POST /ai/face/recognize — multipart: face
        /// </summary>
        public string RecognizePath { get; set; } = "/ai/face/recognize";

        /// <summary>
        /// Request timeout in seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 120;

        /// <summary>
        /// Maximum image size in bytes (10 MB default).
        /// </summary>
        public long MaxImageBytes { get; set; } = 10 * 1024 * 1024;

        /// <summary>
        /// Minimum similarity threshold for recognition match.
        /// AI returns its own threshold; this is a secondary BE check.
        /// </summary>
        public double SimilarityThreshold { get; set; } = 0.80;
    }
}
