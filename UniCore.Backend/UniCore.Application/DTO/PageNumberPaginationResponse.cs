namespace UniCore.Application.DTO
{
    public class PageNumberPaginationResponse<T> : PaginationResponse<T>
    {
        public int PageNumber { get; init; }
        public int TotalRecords { get; init; }
        public int TotalPages { get; init; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
