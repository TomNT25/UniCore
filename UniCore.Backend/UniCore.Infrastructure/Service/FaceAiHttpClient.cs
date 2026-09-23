using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UniCore.Application.Contract.Service.v1;
using UniCore.Helper.Constant;
using UniCore.Helper.Options;

namespace UniCore.Infrastructure.Service
{
    /// <summary>
    /// HTTP client implementation for Face Recognition AI service.
    /// </summary>
    public class FaceAiHttpClient : IFaceAiClient
    {
        private readonly HttpClient _httpClient;
        private readonly FaceAiOptions _options;
        private readonly ILogger<FaceAiHttpClient> _logger;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        public FaceAiHttpClient(
            HttpClient httpClient,
            IOptions<FaceAiOptions> options,
            ILogger<FaceAiHttpClient> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;

            _httpClient.BaseAddress = new Uri(_options.BaseUrl.TrimEnd('/'));
            _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
        }

        public async Task<FaceEnrollResult> EnrollAsync(
            string userId,
            string username,
            IReadOnlyList<FaceImageInput> faceImages,
            CancellationToken cancellationToken = default)
        {
            if (faceImages == null || faceImages.Count == 0)
            {
                return new FaceEnrollResult
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.MissingImages,
                    ErrorMessage = "At least one face image is required"
                };
            }

            if (faceImages.Count > 5)
            {
                return new FaceEnrollResult
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InvalidImageType,
                    ErrorMessage = "Maximum 5 face images allowed"
                };
            }

            try
            {
                using var content = new MultipartFormDataContent();

                // Add user_id and username
                content.Add(new StringContent(userId), "user_id");
                content.Add(new StringContent(username), "username");

                // Add face images
                for (int i = 0; i < faceImages.Count; i++)
                {
                    var image = faceImages[i];

                    // Validate content type
                    if (!FaceAuthConstants.AllowedContentTypes.Contains(image.ContentType))
                    {
                        return new FaceEnrollResult
                        {
                            Success = false,
                            ErrorCode = FaceAuthConstants.ErrorCodes.InvalidImageType,
                            ErrorMessage = $"Invalid content type for image {i + 1}: {image.ContentType}"
                        };
                    }

                    // Validate size
                    if (image.Length > _options.MaxImageBytes)
                    {
                        return new FaceEnrollResult
                        {
                            Success = false,
                            ErrorCode = FaceAuthConstants.ErrorCodes.ImageTooLarge,
                            ErrorMessage = $"Image {i + 1} exceeds maximum size"
                        };
                    }

                    var streamContent = new StreamContent(image.Stream);
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                    content.Add(streamContent, $"face_{i + 1}", image.FileName);
                }

                _logger.LogInformation(
                    "Calling Face AI enroll for user {UserId} with {ImageCount} images",
                    userId, faceImages.Count);

                var response = await _httpClient.PostAsync(_options.EnrollPath, content, cancellationToken);
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                _logger.LogDebug("Face AI enroll response: {StatusCode} - {Body}",
                    response.StatusCode, responseBody);

                var envelope = JsonSerializer.Deserialize<FaceAiEnvelope>(responseBody, JsonOptions);

                if (envelope == null)
                {
                    return new FaceEnrollResult
                    {
                        Success = false,
                        ErrorCode = FaceAuthConstants.ErrorCodes.InternalError,
                        ErrorMessage = "Invalid response from Face AI"
                    };
                }

                if (envelope.Success && envelope.Data != null)
                {
                    return new FaceEnrollResult
                    {
                        Success = true,
                        RequestId = envelope.RequestId,
                        ModelVersion = envelope.ModelVersion,
                        EmbeddingId = envelope.Data.EmbeddingId,
                        UserId = envelope.Data.UserId,
                        Username = envelope.Data.Username,
                        NumImagesUsed = envelope.Data.NumImagesUsed ?? 1
                    };
                }
                else
                {
                    return new FaceEnrollResult
                    {
                        Success = false,
                        RequestId = envelope.RequestId,
                        ModelVersion = envelope.ModelVersion,
                        ErrorCode = envelope.Error?.ErrorCode ?? "UNKNOWN_ERROR",
                        ErrorMessage = envelope.Error?.Message,
                        Stage = envelope.Error?.Stage
                    };
                }
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Face AI enroll request timed out for user {UserId}", userId);
                return new FaceEnrollResult
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InternalError,
                    ErrorMessage = "Request timed out"
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Face AI enroll request failed for user {UserId}", userId);
                return new FaceEnrollResult
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InternalError,
                    ErrorMessage = "Failed to connect to Face AI service"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during face enrollment for user {UserId}", userId);
                return new FaceEnrollResult
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InternalError,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<FaceRecognizeResult> RecognizeAsync(
            FaceImageInput faceImage,
            CancellationToken cancellationToken = default)
        {
            if (faceImage == null || faceImage.Stream == null || faceImage.Stream == Stream.Null)
            {
                return new FaceRecognizeResult
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.MissingImages,
                    ErrorMessage = "Face image is required"
                };
            }

            // Validate content type
            if (!FaceAuthConstants.AllowedContentTypes.Contains(faceImage.ContentType))
            {
                return new FaceRecognizeResult
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InvalidImageType,
                    ErrorMessage = $"Invalid content type: {faceImage.ContentType}"
                };
            }

            // Validate size
            if (faceImage.Length > _options.MaxImageBytes)
            {
                return new FaceRecognizeResult
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.ImageTooLarge,
                    ErrorMessage = "Image exceeds maximum size"
                };
            }

            try
            {
                using var content = new MultipartFormDataContent();

                var streamContent = new StreamContent(faceImage.Stream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(faceImage.ContentType);
                content.Add(streamContent, "face", faceImage.FileName);

                _logger.LogInformation("Calling Face AI recognize");

                var response = await _httpClient.PostAsync(_options.RecognizePath, content, cancellationToken);
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                _logger.LogDebug("Face AI recognize response: {StatusCode} - {Body}",
                    response.StatusCode, responseBody);

                var envelope = JsonSerializer.Deserialize<FaceAiEnvelope>(responseBody, JsonOptions);

                if (envelope == null)
                {
                    return new FaceRecognizeResult
                    {
                        Success = false,
                        ErrorCode = FaceAuthConstants.ErrorCodes.InternalError,
                        ErrorMessage = "Invalid response from Face AI"
                    };
                }

                if (envelope.Success && envelope.Data != null)
                {
                    return new FaceRecognizeResult
                    {
                        Success = true,
                        RequestId = envelope.RequestId,
                        ModelVersion = envelope.ModelVersion,
                        UserId = envelope.Data.UserId,
                        Username = envelope.Data.Username,
                        Similarity = envelope.Data.Similarity ?? 0,
                        Threshold = envelope.Data.Threshold ?? _options.SimilarityThreshold,
                        Status = envelope.Data.Status
                    };
                }
                else
                {
                    return new FaceRecognizeResult
                    {
                        Success = false,
                        RequestId = envelope.RequestId,
                        ModelVersion = envelope.ModelVersion,
                        ErrorCode = envelope.Error?.ErrorCode ?? "UNKNOWN_ERROR",
                        ErrorMessage = envelope.Error?.Message,
                        Stage = envelope.Error?.Stage
                    };
                }
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Face AI recognize request timed out");
                return new FaceRecognizeResult
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InternalError,
                    ErrorMessage = "Request timed out"
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Face AI recognize request failed");
                return new FaceRecognizeResult
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InternalError,
                    ErrorMessage = "Failed to connect to Face AI service"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during face recognition");
                return new FaceRecognizeResult
                {
                    Success = false,
                    ErrorCode = FaceAuthConstants.ErrorCodes.InternalError,
                    ErrorMessage = ex.Message
                };
            }
        }

        #region Response Models

        private class FaceAiEnvelope
        {
            public bool Success { get; set; }
            public string? RequestId { get; set; }
            public string? ModelVersion { get; set; }
            public string? Timestamp { get; set; }
            public FaceAiData? Data { get; set; }
            public FaceAiError? Error { get; set; }
        }

        private class FaceAiData
        {
            public string? EmbeddingId { get; set; }
            public string? UserId { get; set; }
            public string? Username { get; set; }
            public int? NumImagesUsed { get; set; }
            public double? Similarity { get; set; }
            public double? Threshold { get; set; }
            public string? Status { get; set; }
        }

        private class FaceAiError
        {
            public string? ErrorCode { get; set; }
            public string? Message { get; set; }
            public string? Stage { get; set; }
        }

        #endregion
    }
}
