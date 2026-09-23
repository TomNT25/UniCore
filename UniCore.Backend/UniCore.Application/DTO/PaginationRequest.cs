namespace UniCore.Application.DTO
{
    public abstract class PaginationRequest
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = Math.Clamp(value, 1, MaxPageSize);
        }

        public string? SearchTerm { get; set; }
        public string? SortColumn { get; set; }
        public bool SortDescending { get; set; }
    }
}
