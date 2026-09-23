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

            AiOcrApiEnvelope? envelope;
            try
            {
                envelope = JsonSerializer.Deserialize<AiOcrApiEnvelope>(body, JsonOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "AI OCR returned non-JSON body: {Body}", body);
                throw new InvalidOperationException("AI OCR returned an invalid response.");
            }

            if (envelope == null)
            {
                throw new InvalidOperationException("AI OCR returned an empty response.");
            }

            if (!envelope.Success || envelope.Error != null)
            {
                var code = envelope.Error?.Code ?? "OCR_FAILED";
                var message = envelope.Error?.Message ?? "OCR failed.";
                _logger.LogWarning(
                    "AI OCR rejected scan for user {UserId}: {ErrorCode} — {Message}",
                    userId, code, message);
                throw new InvalidOperationException($"{code}: {message}");
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "AI OCR HTTP {StatusCode} for user {UserId}: {Body}",
                    (int)response.StatusCode, userId, body);
                throw new InvalidOperationException(
                    $"AI OCR service returned {(int)response.StatusCode}.");
            }

            var data = envelope.Data
                       ?? throw new InvalidOperationException("AI OCR success response has no data.");

            if (string.IsNullOrWhiteSpace(data.IdNumber) || string.IsNullOrWhiteSpace(data.FullName))
            {
                throw new InvalidOperationException("AI OCR response is missing id_number or full_name.");
            }

            return new AiOcrScanResult
            {
                IdNumber = data.IdNumber.Trim(),
                FullName = data.FullName.Trim(),
                DateOfBirth = data.DateOfBirth,
                Sex = data.Sex,
                Nationality = data.Nationality,
                PlaceOfOrigin = data.PlaceOfOrigin,
                PlaceOfResidence = data.PlaceOfResidence,
                DateOfExpiry = data.DateOfExpiry
            };
        }

        private sealed class AiOcrApiEnvelope
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }

            [JsonPropertyName("data")]
            public AiOcrApiDataDto? Data { get; set; }

            [JsonPropertyName("error")]
            public AiOcrApiErrorDto? Error { get; set; }
        }

        private sealed class AiOcrApiDataDto
        {
            [JsonPropertyName("id_number")]
            public string? IdNumber { get; set; }

            [JsonPropertyName("full_name")]
            public string? FullName { get; set; }

            [JsonPropertyName("date_of_birth")]
            public string? DateOfBirth { get; set; }

            [JsonPropertyName("sex")]
            public string? Sex { get; set; }

            [JsonPropertyName("nationality")]
            public string? Nationality { get; set; }

            [JsonPropertyName("place_of_origin")]
            public string? PlaceOfOrigin { get; set; }

            [JsonPropertyName("place_of_residence")]
            public string? PlaceOfResidence { get; set; }

            [JsonPropertyName("date_of_expiry")]
            public string? DateOfExpiry { get; set; }
        }

        private sealed class AiOcrApiErrorDto
        {
            [JsonPropertyName("code")]
            public string? Code { get; set; }

            [JsonPropertyName("message")]
            public string? Message { get; set; }
        }
    }
}
