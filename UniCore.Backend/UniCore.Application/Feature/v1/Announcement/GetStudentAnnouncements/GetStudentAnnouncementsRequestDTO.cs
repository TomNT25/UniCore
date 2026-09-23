using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Announcement.GetStudentAnnouncements
{
    public class GetStudentAnnouncementsRequestDTO : PageNumberPaginationRequest, IRequest<GetStudentAnnouncementsResponseDTO>
    {
        /// <summary>
        /// Student ID from JWT token (set by controller).
        /// </summary>
        public string StudentId { get; set; } = string.Empty;

        /// <summary>
        /// Filter by read status: null = all, true = read, false = unread.
        /// </summary>
        public bool? IsRead { get; set; }

        /// <summary>
        /// Filter by type: NORMAL, IMPORTANT, URGENT.
        /// </summary>
        public string? Type { get; set; }
    }
}
