namespace UniCore.Helper.Constant
{
    public static class AiOcrConstants
    {
        public const string ImageQualityRejected = "IMAGE_QUALITY_REJECTED";

        public static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/png",
            "image/jpeg",
            "image/jpg"
        };

        public static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".png",
            ".jpg",
            ".jpeg"
        };
    }
}
