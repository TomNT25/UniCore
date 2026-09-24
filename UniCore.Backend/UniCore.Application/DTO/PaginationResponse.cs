using System.Collections.Generic;

namespace UniCore.Application.DTO
{
    public abstract class PaginationResponse<T, TMetadata>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public TMetadata Metadata { get; set; } = default!;
    }
}
