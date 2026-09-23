using System.Text.Json.Serialization;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Announcement.GetStudentAnnouncements
{
    public class GetStudentAnnouncementsResponseDTO : PageNumberPaginationResponse<StudentAnnouncementItemDTO>
    {
        public int UnreadCount { get; set; }
    }

    public class StudentAnnouncementItemDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("scope_type")]
        public string ScopeType { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("publish_date")]
        public DateTime PublishDate { get; set; }

        [JsonPropertyName("expired_date")]
        public DateTime ExpiredDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        // Student-specific read status
        public bool IsRead { get; set; }
        public DateTime? ViewedAt { get; set; }
        public bool IsAcknowledged { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
    }
}
