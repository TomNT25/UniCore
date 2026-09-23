using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.ClassRoom.GetClassInfos
{
    public class GetClassInfoRequestDTO : IRequest<GetClassInfoResponseDTO>
    {
        public string ClassID { get; set; }
    }
}
