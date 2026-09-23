using System.Text.Json.Serialization;

namespace UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement
{
    /// <summary>
    /// Admin list/detail announcement shape aligned with FE contract (UTC datetimes; client converts timezone).
    /// </summary>
    public class AdminAnnouncementItemDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        /// <summary>Full markdown; omitted on list API to keep DataTable payload small.</summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Content { get; set; }

        public string Type { get; set; } = "NORMAL";

        [JsonPropertyName("publish_date")]
        public DateTime PublishDate { get; set; }

        [JsonPropertyName("expired_date")]
        public DateTime ExpiredDate { get; set; }

        [JsonPropertyName("recipient_count")]
        public int? RecipientCount { get; set; }

        [JsonPropertyName("scope_type")]
        public string ScopeType { get; set; } = "PUBLIC";

        public string Status { get; set; } = "UPCOMING";
    }
}
