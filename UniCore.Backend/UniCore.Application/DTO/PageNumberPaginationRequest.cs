namespace UniCore.Application.DTO
{
    public class PageNumberPaginationRequest : PaginationRequest
    {
        private int _pageNumber = 1;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = Math.Max(1, value);
        }
    }
}
