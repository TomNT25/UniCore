using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UniCore.Application.DTO
{
    public class CursorPaginationMetaResponse
    {
        public int PageSize { get; set; }
        public string? NextCursor { get; set; }
        public bool HasNextPage { get; set; }
    }

    public class CursorPaginationResponse<T, TMetadata> : PaginationResponse<T, TMetadata>
        where TMetadata : CursorPaginationMetaResponse, new()
    {
        public CursorPaginationResponse()
        {
            Metadata = new TMetadata();
        }

        public CursorPaginationResponse(IEnumerable<T> items, TMetadata metadata)
        {
            Items = items;
            Metadata = metadata;
        }

        [JsonIgnore]
        public int PageSize
        {
            get => Metadata?.PageSize ?? 0;
            set => EnsureMetadata().PageSize = value;
        }

        [JsonIgnore]
        public string? NextCursor
        {
            get => Metadata?.NextCursor;
            set => EnsureMetadata().NextCursor = value;
        }

        [JsonIgnore]
        public bool HasNextPage
        {
            get => Metadata?.HasNextPage ?? false;
            set => EnsureMetadata().HasNextPage = value;
        }

        protected TMetadata EnsureMetadata()
        {
            return Metadata ??= new TMetadata();
        }
    }

    public class CursorPaginationResponse<T> : CursorPaginationResponse<T, CursorPaginationMetaResponse>
    {
        public CursorPaginationResponse() : base()
        {
        }

        public CursorPaginationResponse(IEnumerable<T> items, CursorPaginationMetaResponse metadata)
            : base(items, metadata)
        {
        }
    }
}
