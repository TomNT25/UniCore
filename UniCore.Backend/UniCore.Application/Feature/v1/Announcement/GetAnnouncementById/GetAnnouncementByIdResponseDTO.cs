using System.Text.Json.Serialization;
using UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement;

namespace UniCore.Application.Feature.v1.Announcement.GetAnnouncementById
{
    public class GetAnnouncementByIdResponseDTO
    {
        public AdminAnnouncementItemDTO? Announcement { get; set; }

        [JsonPropertyName("targets")]
        public List<AdminAnnouncementTargetDto> Targets { get; set; } = new();

        [JsonPropertyName("sent_email")]
        public List<AdminAnnouncementSentEmailDto> SentEmail { get; set; } = new();
    }

    public class AdminAnnouncementTargetDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("personal_email")]
        public string PersonalEmail { get; set; } = string.Empty;
    }

    public class AdminAnnouncementSentEmailDto
    {
        [JsonPropertyName("sent_date")]
        public DateTime? SentDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
