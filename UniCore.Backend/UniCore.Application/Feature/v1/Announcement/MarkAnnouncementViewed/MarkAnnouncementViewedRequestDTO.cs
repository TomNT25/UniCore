using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Announcement.MarkAnnouncementViewed
{
    public class MarkAnnouncementViewedRequestDTO : IRequest<MarkAnnouncementViewedResponseDTO>
    {
        /// <summary>
        /// Announcement ID.
        /// </summary>
        public string AnnouncementId { get; set; } = string.Empty;

        /// <summary>
        /// Student ID from JWT token (set by controller).
        /// </summary>
        public string StudentId { get; set; } = string.Empty;
    }
}
