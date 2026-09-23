namespace UniCore.Helper.Options
{
    public class AiOcrOptions
    {
        public const string SectionName = "AiOcr";

        /// <summary>Base URL of MaivenPoint OCR FastAPI, e.g. http://127.0.0.1:8000</summary>
        public string BaseUrl { get; set; } = "http://127.0.0.1:8000";

        /// <summary>From OCR README / Swagger: POST /ocr</summary>
        public string ScanPath { get; set; } = "/ocr";

        /// <summary>Multipart field name expected by AI API.</summary>
        public string ImageFieldName { get; set; } = "file";

        /// <summary>
        /// MaivenPoint OCR does not accept user-id — leave empty.
        /// </summary>
        public string UserIdFieldName { get; set; } = string.Empty;

        public int TimeoutSeconds { get; set; } = 60;

        public long MaxImageBytes { get; set; } = 10 * 1024 * 1024;
    }
}
