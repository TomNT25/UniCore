namespace UniCore.Application.Feature.v1.Announcement.MarkAnnouncementViewed
{
    public class MarkAnnouncementViewedResponseDTO
    {
        public bool Success { get; set; }
        public string AnnouncementId { get; set; } = string.Empty;
        public DateTime? ViewedAt { get; set; }
    }
}
