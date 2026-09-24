using System.Text.Json.Serialization;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Announcement.CreateAnnouncement
{
    public class CreateAnnouncementRequestDTO : IRequest<CreateAnnouncementResponseDTO>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("scope_type")]
        public string ScopeType { get; set; } = string.Empty;
        [JsonPropertyName("scope_value")]
        public string? ScopeValue { get; set; }
        
        /// <summary>Required when ScopeType = SPECIFIC_STUDENTS.</summary>
        public List<string> TargetStudentIds { get; set; } = new();
        public bool RequireAcknowledgement { get; set; }

        [JsonPropertyName("publish_date")]
        public DateTime PublishDate { get; set; }

        [JsonPropertyName("expired_date")]
        public DateTime ExpiredDate { get; set; }

        [JsonIgnore]
        public string? ActorUserId { get; set; }
    }
}
