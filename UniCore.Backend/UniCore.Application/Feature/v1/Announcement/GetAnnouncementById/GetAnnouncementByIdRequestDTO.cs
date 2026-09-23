using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Announcement.GetAnnouncementById
{
    public class GetAnnouncementByIdRequestDTO : IRequest<GetAnnouncementByIdResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
    }
}
