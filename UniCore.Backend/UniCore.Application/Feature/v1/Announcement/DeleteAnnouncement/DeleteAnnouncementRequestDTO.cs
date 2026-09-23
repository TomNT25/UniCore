using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Announcement.DeleteAnnouncement
{
    public class DeleteAnnouncementRequestDTO : IRequest<DeleteAnnouncementResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
    }
}
