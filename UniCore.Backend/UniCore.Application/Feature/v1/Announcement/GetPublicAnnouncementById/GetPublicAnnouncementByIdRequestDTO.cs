using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncementById
{
    public class GetPublicAnnouncementByIdRequestDTO : IRequest<GetPublicAnnouncementByIdResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
    }
}
