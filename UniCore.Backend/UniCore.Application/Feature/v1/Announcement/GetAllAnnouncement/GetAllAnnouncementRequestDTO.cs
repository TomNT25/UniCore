using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement
{
    public class GetAllAnnouncementRequestDTO : PageNumberPaginationRequest, IRequest<GetAllAnnouncementResponseDTO>
    {
        public string? Status { get; set; }
    }
}
