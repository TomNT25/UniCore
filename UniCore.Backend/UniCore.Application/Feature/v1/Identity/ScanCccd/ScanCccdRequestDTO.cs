using System.Text.Json.Serialization;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Identity.ScanCccd
{
    public class ScanCccdRequestDTO : IRequest<ScanCccdResponseDTO>
    {
        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;

        [JsonIgnore]
        public Stream? ImageStream { get; set; }

        [JsonIgnore]
        public string FileName { get; set; } = string.Empty;

        [JsonIgnore]
        public string ContentType { get; set; } = string.Empty;

        [JsonIgnore]
        public long FileLength { get; set; }
    }
}
