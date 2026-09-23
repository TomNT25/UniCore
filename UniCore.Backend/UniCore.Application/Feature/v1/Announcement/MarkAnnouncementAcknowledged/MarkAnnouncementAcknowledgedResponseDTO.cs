namespace UniCore.Application.Feature.v1.Announcement.MarkAnnouncementAcknowledged
{
    public class MarkAnnouncementAcknowledgedResponseDTO
    {
        public bool Success { get; set; }
        public string AnnouncementId { get; set; } = string.Empty;
        public DateTime? AcknowledgedAt { get; set; }
    }
}
