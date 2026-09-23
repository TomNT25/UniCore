namespace UniCore.Application.Feature.v1.Announcement.GetDeliveryReport
{
    public class GetDeliveryReportResponseDTO
    {
        public string AnnouncementId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int RecipientCount { get; set; }
        public int ViewedCount { get; set; }
        public int AcknowledgedCount { get; set; }
        public List<DeliveryRecipientDTO> Recipients { get; set; } = new();
    }

    public class DeliveryRecipientDTO
    {
        public string StudentId { get; set; } = string.Empty;
        public string? Username { get; set; }
        public string? Email { get; set; }
        public DateTime? ViewedAt { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
    }
}
