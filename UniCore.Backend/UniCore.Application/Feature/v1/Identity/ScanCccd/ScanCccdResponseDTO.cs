namespace UniCore.Application.Feature.v1.Identity.ScanCccd
{
    public class ScanCccdResponseDTO
    {
        public string IdNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? DateOfBirth { get; set; }
        public string? Sex { get; set; }
        public string? Nationality { get; set; }
        public string? PlaceOfOrigin { get; set; }
        public string? PlaceOfResidence { get; set; }
        public string? DateOfExpiry { get; set; }

        public string VerificationStatus { get; set; } = "VERIFIED";
        public DateTime? VerifiedAt { get; set; }
        public string? PersonIdRecordId { get; set; }
    }
}
