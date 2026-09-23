namespace UniCore.Application.Entity
{
    public class UserPersonId
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Code { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string IdNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string CardType { get; set; } = "CCCD_CHIP";
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; } = "Việt Nam";
        public string? PlaceOfOrigin { get; set; }
        public string? PlaceOfResidence { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? IssuePlace { get; set; }
        public string? FrontImageUrl { get; set; }
        public string? BackImageUrl { get; set; }
        public string VerificationStatus { get; set; } = "UNVERIFIED";
        public DateTime? VerifiedAt { get; set; }
        public string? VerifiedBy { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
