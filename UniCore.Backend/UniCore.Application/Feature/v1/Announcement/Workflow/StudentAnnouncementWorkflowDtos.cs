using System.Text.Json.Serialization;

namespace UniCore.Application.Feature.v1.Announcement.Workflow
{
    public class StudentAnnouncementWorkflowListResponse
    {
        public List<StudentAnnouncementWorkflowItemDto> Items { get; set; } = new();
    }

    public class StudentAnnouncementWorkflowItemDto
    {
        [JsonPropertyName("announcement_id")]
        public string AnnouncementId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Type { get; set; } = "NORMAL";

        /// <summary>Computed lifecycle status (UPCOMING/ACTIVE/EXPIRED).</summary>
        [JsonPropertyName("st")]
        public string St { get; set; } = "UPCOMING";

        [JsonPropertyName("scope_type")]
        public string ScopeType { get; set; } = "PUBLIC";

        [JsonPropertyName("scope_value")]
        public string? ScopeValue { get; set; }

        [JsonPropertyName("publish_date")]
        public DateTime PublishDate { get; set; }

        [JsonPropertyName("expired_date")]
        public DateTime ExpiredDate { get; set; }

        public string? Content { get; set; }

        [JsonPropertyName("viewed_at")]
        public DateTime? ViewedAt { get; set; }

        [JsonPropertyName("acknowledged_at")]
        public DateTime? AcknowledgedAt { get; set; }
    }

    public class ConfirmAcknowledgedRequestDto
    {
        [JsonPropertyName("announcement_id")]
        public string AnnouncementId { get; set; } = string.Empty;
    }
}
