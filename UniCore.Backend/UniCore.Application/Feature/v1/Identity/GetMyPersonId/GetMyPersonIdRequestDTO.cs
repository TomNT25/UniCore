using System.Text.Json.Serialization;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Identity.GetMyPersonId
{
    public class GetMyPersonIdRequestDTO : IRequest<GetMyPersonIdResponseDTO>
    {
        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;
    }
}
