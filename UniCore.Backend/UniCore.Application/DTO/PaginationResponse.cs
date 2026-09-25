using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UniCore.Application.DTO
{
    public abstract class PaginationResponse<T, TMetadata>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public TMetadata Metadata { get; set; } = default!;

        [JsonIgnore]
        public TMetadata MetaData
        {
            get => Metadata;
            set => Metadata = value;
        }
    }
}
