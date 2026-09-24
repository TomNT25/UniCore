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

                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                bool isSuccess = false;
                if (root.TryGetProperty("isSuccess", out var isSuccessProp) || root.TryGetProperty("success", out isSuccessProp))
                {
                    isSuccess = isSuccessProp.GetBoolean();
                }
                else if (root.TryGetProperty("statusCode", out var statusProp) && statusProp.GetInt32() == 0)
                {
                    isSuccess = true;
                }
                else if (response.IsSuccessStatusCode)
                {
                    isSuccess = true;
                }

                if (isSuccess && response.IsSuccessStatusCode)
                {
                    JsonElement dataElem = root;
                    if (root.TryGetProperty("data", out var dElem))
                    {
                        if (dElem.TryGetProperty("items", out var itemsElem))
                        {
                            dataElem = itemsElem;
                        }
                        else
                        {
                            dataElem = dElem;
                        }
                    }

                    var embeddingId = GetPropString(dataElem, "embedding_id") ?? GetPropString(root, "embedding_id") ?? Guid.NewGuid().ToString();
                    var resUserId = GetPropString(dataElem, "user_id") ?? GetPropString(root, "user_id") ?? userId;
                    var resUsername = GetPropString(dataElem, "username") ?? GetPropString(root, "username") ?? username;
                    var modelVersion = GetPropString(root, "model_version") ?? GetPropString(dataElem, "model_version") ?? "buffalo_l_v1";
                    var requestId = GetPropString(root, "request_id") ?? GetPropString(dataElem, "request_id");

                    return new FaceEnrollResult
                    {
                        Success = true,
                        RequestId = requestId,
                        ModelVersion = modelVersion,
                        EmbeddingId = embeddingId,
                        UserId = resUserId,
                        Username = resUsername,
                        NumImagesUsed = faceImages.Count
                    };
                }
                else
                {
                    var (errCode, errMsg) = ExtractError(root);
                    return new FaceEnrollResult
                    {
                        Success = false,
                        RequestId = GetPropString(root, "request_id"),
                        ModelVersion = GetPropString(root, "model_version"),
                        ErrorCode = errCode ?? FaceAuthConstants.ErrorCodes.InternalError,
                        ErrorMessage = errMsg ?? "Face enrollment failed"
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

                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                bool isSuccess = false;
                if (root.TryGetProperty("isSuccess", out var isSuccessProp) || root.TryGetProperty("success", out isSuccessProp))
                {
                    isSuccess = isSuccessProp.GetBoolean();
                }
                else if (root.TryGetProperty("statusCode", out var statusProp) && statusProp.GetInt32() == 0)
                {
                    isSuccess = true;
                }
                else if (response.IsSuccessStatusCode)
                {
                    isSuccess = true;
                }

                if (isSuccess && response.IsSuccessStatusCode)
                {
                    JsonElement dataElem = root;
                    if (root.TryGetProperty("data", out var dElem))
                    {
                        if (dElem.TryGetProperty("items", out var itemsElem))
                        {
                            dataElem = itemsElem;
                        }
                        else
                        {
                            dataElem = dElem;
                        }
                    }

                    var resUserId = GetPropString(dataElem, "user_id") ?? GetPropString(root, "user_id");
                    var resUsername = GetPropString(dataElem, "username") ?? GetPropString(root, "username");
                    var similarity = GetPropDouble(dataElem, "similarity") ?? GetPropDouble(root, "similarity") ?? 0;
                    var threshold = GetPropDouble(dataElem, "threshold") ?? GetPropDouble(root, "threshold") ?? _options.SimilarityThreshold;
                    var status = GetPropString(dataElem, "status") ?? GetPropString(root, "status");
                    var requestId = GetPropString(root, "request_id") ?? GetPropString(dataElem, "request_id");
                    var modelVersion = GetPropString(root, "model_version") ?? GetPropString(dataElem, "model_version") ?? "buffalo_l_v1";

                    return new FaceRecognizeResult
                    {
                        Success = true,
                        RequestId = requestId,
                        ModelVersion = modelVersion,
                        UserId = resUserId,
                        Username = resUsername,
                        Similarity = similarity,
                        Threshold = threshold,
                        Status = status
                    };
                }
                else
                {
                    var (errCode, errMsg) = ExtractError(root);
                    return new FaceRecognizeResult
                    {
                        Success = false,
                        RequestId = GetPropString(root, "request_id"),
                        ModelVersion = GetPropString(root, "model_version"),
                        ErrorCode = errCode ?? FaceAuthConstants.ErrorCodes.InternalError,
                        ErrorMessage = errMsg ?? "Face recognition failed"
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

        private static string? GetPropString(JsonElement element, string propName)
        {
            if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty(propName, out var prop))
            {
                return prop.GetString();
            }
            return null;
        }

        private static double? GetPropDouble(JsonElement element, string propName)
        {
            if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty(propName, out var prop))
            {
                if (prop.ValueKind == JsonValueKind.Number && prop.TryGetDouble(out var d))
                {
                    return d;
                }
            }
            return null;
        }

        private static (string? ErrorCode, string? ErrorMessage) ExtractError(JsonElement root)
        {
            string? errorCode = null;
            string? errorMessage = null;

            if (root.TryGetProperty("errors", out var errorsProp) && errorsProp.ValueKind == JsonValueKind.Array && errorsProp.GetArrayLength() > 0)
            {
                var first = errorsProp[0];
                if (first.ValueKind == JsonValueKind.String)
                {
                    errorMessage = first.GetString();
                }
                else if (first.ValueKind == JsonValueKind.Object)
                {
                    if (first.TryGetProperty("code", out var c)) errorCode = c.GetString();
                    if (first.TryGetProperty("detail", out var d)) errorMessage = d.GetString();
                    else if (first.TryGetProperty("message", out var m)) errorMessage = m.GetString();
                }
            }

            if (string.IsNullOrWhiteSpace(errorMessage) && root.TryGetProperty("error", out var errorProp))
            {
                if (errorProp.ValueKind == JsonValueKind.Object)
                {
                    if (errorProp.TryGetProperty("error_code", out var ec)) errorCode = ec.GetString();
                    else if (errorProp.TryGetProperty("code", out var c)) errorCode = c.GetString();

                    if (errorProp.TryGetProperty("message", out var m)) errorMessage = m.GetString();
                    else if (errorProp.TryGetProperty("detail", out var d)) errorMessage = d.GetString();
                }
                else if (errorProp.ValueKind == JsonValueKind.String)
                {
                    errorMessage = errorProp.GetString();
                }
            }

            if (string.IsNullOrWhiteSpace(errorMessage) && root.TryGetProperty("message", out var msgProp))
            {
                errorMessage = msgProp.GetString();
            }

            return (errorCode, errorMessage);
        }
    }
}
