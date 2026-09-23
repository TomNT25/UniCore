using System.Text.Json.Serialization;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Announcement.PreviewRecipients
{
    public class PreviewRecipientsRequestDTO : IRequest<PreviewRecipientsResponseDTO>
    {
        public string ScopeType { get; set; } = string.Empty;
        public string? ScopeValue { get; set; }

        [JsonPropertyName("targets")]
        public List<string> Targets { get; set; } = new();

        public List<string> TargetStudentIds { get; set; } = new();
    }
}
