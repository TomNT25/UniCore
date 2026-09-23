namespace UniCore.Application.Feature.v1.Identity.GetMyPersonId
{
    public class GetMyPersonIdResponseDTO
    {
        public bool HasRecord { get; set; }
        public string? Id { get; set; }
        public string? IdNumber { get; set; }
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? PlaceOfOrigin { get; set; }
        public string? PlaceOfResidence { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string VerificationStatus { get; set; } = "UNVERIFIED";
        public DateTime? VerifiedAt { get; set; }
    }
}
