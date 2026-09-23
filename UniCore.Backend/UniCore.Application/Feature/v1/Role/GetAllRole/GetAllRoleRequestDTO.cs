using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Role.GetAllRole
{
    public class GetAllRoleRequestDTO : PageNumberPaginationRequest, IRequest<GetAllRoleResponseDTO>
    {
    }
}
