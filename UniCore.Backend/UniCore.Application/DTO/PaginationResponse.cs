using System.Collections.Generic;

namespace UniCore.Application.DTO
{
    public abstract class PaginationResponse<T>
    {
        public IEnumerable<T> Items { get; init; } = [];
        public int PageSize { get; init; }
    }
}
