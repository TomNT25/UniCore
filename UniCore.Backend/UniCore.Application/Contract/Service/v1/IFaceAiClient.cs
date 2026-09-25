using System.Text.Json;
using System.Text.Json.Serialization;

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
    /// User details returned in Face AI response items.
    /// </summary>
    public class FaceAiUserItem
    {
        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }
    }

    /// <summary>
    /// Data container for Face AI response payload.
    /// </summary>
    public class FaceAiDataResult
    {
        [JsonPropertyName("items")]
        public FaceAiUserItem? Items { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    /// <summary>
    /// Result from face enrollment API matching the AI Service response contract.
    /// </summary>
    public class FaceEnrollResult
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public FaceAiDataResult? Data { get; set; }

        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; } = new();

        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; }

        // Backward-compatible properties for existing handlers
        [JsonIgnore]
        public bool Success
        {
            get => IsSuccess;
            set => IsSuccess = value;
        }

        private string? _userId;
        [JsonIgnore]
        public string? UserId
        {
            get => _userId ?? Data?.Items?.UserId;
            set
            {
                _userId = value;
                if (Data?.Items != null)
                {
                    Data.Items.UserId = value;
                }
            }
        }

        private string? _username;
        [JsonIgnore]
        public string? Username
        {
            get => _username ?? Data?.Items?.Username;
            set
            {
                _username = value;
                if (Data?.Items != null)
                {
                    Data.Items.Username = value;
                }
            }
        }

        [JsonIgnore]
        public string? ErrorCode { get; set; }

        private string? _errorMessage;
        [JsonIgnore]
        public string? ErrorMessage
        {
            get => _errorMessage ?? Message ?? (Errors.Count > 0 ? Errors[0] : null);
            set => _errorMessage = value;
        }

        [JsonIgnore]
        public string? Stage { get; set; }
    }

    /// <summary>
    /// Result from face recognition API matching the AI Service response contract.
    /// </summary>
    public class FaceRecognizeResult
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public FaceAiDataResult? Data { get; set; }

        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; } = new();

        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; }

        // Backward-compatible properties for existing handlers
        [JsonIgnore]
        public bool Success
        {
            get => IsSuccess;
            set => IsSuccess = value;
        }

        private string? _userId;
        [JsonIgnore]
        public string? UserId
        {
            get => _userId ?? Data?.Items?.UserId;
            set
            {
                _userId = value;
                if (Data?.Items != null)
                {
                    Data.Items.UserId = value;
                }
            }
        }

        private string? _username;
        [JsonIgnore]
        public string? Username
        {
            get => _username ?? Data?.Items?.Username;
            set
            {
                _username = value;
                if (Data?.Items != null)
                {
                    Data.Items.Username = value;
                }
            }
        }

        [JsonIgnore]
        public string? ErrorCode { get; set; }

        private string? _errorMessage;
        [JsonIgnore]
        public string? ErrorMessage
        {
            get => _errorMessage ?? Message ?? (Errors.Count > 0 ? Errors[0] : null);
            set => _errorMessage = value;
        }

        [JsonIgnore]
        public string? Stage { get; set; }
    }
}
