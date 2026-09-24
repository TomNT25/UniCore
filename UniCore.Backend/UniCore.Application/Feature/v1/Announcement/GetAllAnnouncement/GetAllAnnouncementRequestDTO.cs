using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement
{
    public class GetAllAnnouncementRequestDTO : PageNumberPaginationRequest, IRequest<GetAllAnnouncementResponseDTO>
    {
        public string? Status { get; set; }

        /// <summary>NORMAL | IMPORTANT | URGENT (query: announcementType)</summary>
        public string? AnnouncementType { get; set; }

        /// <summary>PUBLIC | STUDENTS | DEPARTMENT | CLASS | COURSE | SPECIFIC_STUDENTS</summary>
        public string? ScopeType { get; set; }

        /// <summary>
        /// Lower bound of the filter window (UTC). Keeps announcements whose active period
        /// overlaps the range: ExpiredDate &gt; ActiveFrom.
        /// </summary>
        public DateTime? ActiveFrom { get; set; }

        /// <summary>
        /// Upper bound of the filter window (UTC). Keeps announcements whose active period
        /// overlaps the range: PublishDate &lt;= ActiveTo.
        /// </summary>
        public DateTime? ActiveTo { get; set; }

        /// <summary>FE sort field: startDate, endDate, title, code, type, scopeType, status, createdAt.</summary>
        public string? SortBy { get; set; }

        /// <summary>asc | desc</summary>
        public string? SortOrder { get; set; }
    }
}