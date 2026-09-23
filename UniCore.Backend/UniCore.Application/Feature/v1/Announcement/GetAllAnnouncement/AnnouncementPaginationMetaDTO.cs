namespace UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement
{
    public class AnnouncementPaginationMetaDTO
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }
        public bool HasNextPage { get; init; }
        public bool HasPreviousPage { get; init; }
    }
}
