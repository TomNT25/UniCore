using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UniCore.Application.Contract.Service.v1;
using UniCore.Helper.Options;

namespace UniCore.Infrastructure.Service
{
    /// <summary>
    /// Proxies to MaivenPoint OCR FastAPI:
    /// POST /ocr, multipart field "file",
    /// response envelope { success, data, error }.
    /// </summary>
    public class AiOcrHttpClient : IAiOcrClient
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;
        private readonly AiOcrOptions _options;
        private readonly ILogger<AiOcrHttpClient> _logger;

        public AiOcrHttpClient(
            HttpClient httpClient,
            IOptions<AiOcrOptions> options,
            ILogger<AiOcrHttpClient> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<AiOcrScanResult> ScanAsync(
            string userId,
            Stream imageStream,
            string fileName,
            string contentType,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_options.BaseUrl))
            {
                throw new InvalidOperationException("AiOcr:BaseUrl is not configured.");
            }

            using var content = new MultipartFormDataContent();

            if (!string.IsNullOrWhiteSpace(_options.UserIdFieldName))
            {
                content.Add(new StringContent(userId), _options.UserIdFieldName);
            }

            var streamContent = new StreamContent(imageStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(
                string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType);

            var imageField = string.IsNullOrWhiteSpace(_options.ImageFieldName)
                ? "file"
                : _options.ImageFieldName;
            content.Add(streamContent, imageField, string.IsNullOrWhiteSpace(fileName) ? "cccd.jpg" : fileName);

            var scanPath = string.IsNullOrWhiteSpace(_options.ScanPath) ? "/ocr" : _options.ScanPath;
            var path = scanPath.StartsWith('/') ? scanPath : "/" + scanPath;

            using var response = await _httpClient.PostAsync(path, content, cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            using var doc = JsonDocument.Parse(body);
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

            if (!isSuccess || !response.IsSuccessStatusCode)
            {
                string? errorCode = null;
                string? errorMessage = null;

                if (root.TryGetProperty("errors", out var errorsProp) && errorsProp.ValueKind == JsonValueKind.Array && errorsProp.GetArrayLength() > 0)
                {
                    var firstErr = errorsProp[0];
                    if (firstErr.ValueKind == JsonValueKind.String)
                    {
                        errorMessage = firstErr.GetString();
                    }
                    else if (firstErr.ValueKind == JsonValueKind.Object)
                    {
                        if (firstErr.TryGetProperty("code", out var c)) errorCode = c.GetString();
                        if (firstErr.TryGetProperty("detail", out var d)) errorMessage = d.GetString();
                        else if (firstErr.TryGetProperty("message", out var m)) errorMessage = m.GetString();
                    }
                }

                if (string.IsNullOrWhiteSpace(errorMessage) && root.TryGetProperty("error", out var errorProp))
                {
                    if (errorProp.ValueKind == JsonValueKind.Object)
                    {
                        if (errorProp.TryGetProperty("code", out var c)) errorCode = c.GetString();
                        if (errorProp.TryGetProperty("message", out var m)) errorMessage = m.GetString();
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

                errorCode ??= "OCR_FAILED";
                errorMessage ??= "OCR processing failed.";

                _logger.LogWarning(
                    "AI OCR rejected scan for user {UserId}: {ErrorCode} — {Message}",
                    userId, errorCode, errorMessage);
                throw new InvalidOperationException($"{errorCode}: {errorMessage}");
            }

            // Find target element with data
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

            string? idNumber = GetJsonString(dataElem, "id_number") ?? GetJsonString(root, "id_number");
            string? fullName = GetJsonString(dataElem, "full_name") ?? GetJsonString(root, "full_name");
            string? dateOfBirth = GetJsonString(dataElem, "date_of_birth") ?? GetJsonString(dataElem, "dob") ?? GetJsonString(root, "date_of_birth");
            string? sex = GetJsonString(dataElem, "sex") ?? GetJsonString(dataElem, "gender") ?? GetJsonString(root, "sex");
            string? nationality = GetJsonString(dataElem, "nationality") ?? GetJsonString(root, "nationality");
            string? placeOfOrigin = GetJsonString(dataElem, "place_of_origin") ?? GetJsonString(dataElem, "home_town") ?? GetJsonString(root, "place_of_origin");
            string? placeOfResidence = GetJsonString(dataElem, "place_of_residence") ?? GetJsonString(dataElem, "address") ?? GetJsonString(root, "place_of_residence");
            string? dateOfExpiry = GetJsonString(dataElem, "date_of_expiry") ?? GetJsonString(dataElem, "doe") ?? GetJsonString(root, "date_of_expiry");

            if (string.IsNullOrWhiteSpace(idNumber) || string.IsNullOrWhiteSpace(fullName))
            {
                throw new InvalidOperationException("AI OCR response is missing id_number or full_name.");
            }

            return new AiOcrScanResult
            {
                IdNumber = idNumber.Trim(),
                FullName = fullName.Trim(),
                DateOfBirth = dateOfBirth,
                Sex = sex,
                Nationality = nationality,
                PlaceOfOrigin = placeOfOrigin,
                PlaceOfResidence = placeOfResidence,
                DateOfExpiry = dateOfExpiry
            };
        }

        private static string? GetJsonString(JsonElement element, string propertyName)
        {
            if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty(propertyName, out var prop))
            {
                return prop.GetString();
            }
            return null;
        }
    }
}
