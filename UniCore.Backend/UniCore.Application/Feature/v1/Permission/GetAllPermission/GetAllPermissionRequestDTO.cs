using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO;

namespace UniCore.Application.Feature.v1.Permission.GetAllPermission
{
    public class GetAllPermissionRequestDTO : PageNumberPaginationRequest, IRequest<GetAllPermissionResponseDTO>
    {
    }
}
