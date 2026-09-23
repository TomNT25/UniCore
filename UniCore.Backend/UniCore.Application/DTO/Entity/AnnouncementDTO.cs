namespace UniCore.Application.DTO.Entity
{
    public class AnnouncementDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Type { get; set; } = "NORMAL";
        public string Status { get; set; } = "UPCOMING";
        public string ScopeType { get; set; } = "PUBLIC";
        public string? ScopeValue { get; set; }
        /// <summary>Populated for SPECIFIC_STUDENTS / IMPORTANT / URGENT from announcement_students.</summary>
        public List<string> TargetStudentIds { get; set; } = new();
        public bool RequireAcknowledgement { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int? RecipientCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
