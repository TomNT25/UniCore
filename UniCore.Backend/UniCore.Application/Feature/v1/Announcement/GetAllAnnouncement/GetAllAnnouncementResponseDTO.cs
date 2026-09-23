using System.Text.Json.Serialization;

namespace UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement
{
    public class GetAllAnnouncementResponseDTO
    {
        [JsonPropertyName("data")]
        public List<AdminAnnouncementItemDTO> Data { get; init; } = new();

        [JsonPropertyName("meta")]
        public AnnouncementPaginationMetaDTO Meta { get; init; } = new();
    }
}
