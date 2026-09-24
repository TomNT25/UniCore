using System.Text.Json.Serialization;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement
{
    public class GetAllAnnouncementResponseDTO : PageNumberPaginationResponse<AdminAnnouncementItemDTO>
    {
    }
}
