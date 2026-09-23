namespace UniCore.Application.Contract.Service.v1
{
    public interface IAiOcrClient
    {
        Task<AiOcrScanResult> ScanAsync(
            string userId,
            Stream imageStream,
            string fileName,
            string contentType,
            CancellationToken cancellationToken = default);
    }

    public class AiOcrScanResult
    {
        public string IdNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? DateOfBirth { get; set; }
        public string? Sex { get; set; }
        public string? Nationality { get; set; }
        public string? PlaceOfOrigin { get; set; }
        public string? PlaceOfResidence { get; set; }
        public string? DateOfExpiry { get; set; }
    }
}
