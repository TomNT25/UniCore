using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Announcement.MarkAnnouncementAcknowledged
{
    public class MarkAnnouncementAcknowledgedRequestDTO : IRequest<MarkAnnouncementAcknowledgedResponseDTO>
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
