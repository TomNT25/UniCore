using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Classes.GetClasses
{
    public class GetClassesRequestDTO : PageNumberPaginationRequest, IRequest<GetClassesResponseDTO>
    {
    }
}
