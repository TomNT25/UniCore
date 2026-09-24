using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UniCore.Application.DTO
{
    public class PageNumberPaginationMetaResponse
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }

    public class PageNumberPaginationResponse<T, TMetadata> : PaginationResponse<T, TMetadata>
        where TMetadata : PageNumberPaginationMetaResponse, new()
    {
        public PageNumberPaginationResponse()
        {
            Metadata = new TMetadata();
        }

        public PageNumberPaginationResponse(IEnumerable<T> items, TMetadata metadata)
        {
            Items = items;
            Metadata = metadata;
        }

        [JsonIgnore]
        public int PageNumber
        {
            get => Metadata?.PageNumber ?? 0;
            set => EnsureMetadata().PageNumber = value;
        }

        [JsonIgnore]
        public int PageSize
        {
            get => Metadata?.PageSize ?? 0;
            set => EnsureMetadata().PageSize = value;
        }

        [JsonIgnore]
        public int TotalRecords
        {
            get => Metadata?.TotalRecords ?? 0;
            set => EnsureMetadata().TotalRecords = value;
        }

        [JsonIgnore]
        public int TotalPages
        {
            get => Metadata?.TotalPages ?? 0;
            set => EnsureMetadata().TotalPages = value;
        }

        [JsonIgnore]
        public bool HasPreviousPage
        {
            get => Metadata?.HasPreviousPage ?? false;
            set => EnsureMetadata().HasPreviousPage = value;
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

    public class PageNumberPaginationResponse<T> : PageNumberPaginationResponse<T, PageNumberPaginationMetaResponse>
    {
        public PageNumberPaginationResponse() : base()
        {
        }

        public PageNumberPaginationResponse(IEnumerable<T> items, PageNumberPaginationMetaResponse metadata)
            : base(items, metadata)
        {
        }
    }
}
