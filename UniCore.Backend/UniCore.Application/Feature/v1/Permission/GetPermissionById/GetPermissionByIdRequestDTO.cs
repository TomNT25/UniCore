using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Permission.GetPermissionById
{
    public class GetPermissionByIdRequestDTO : IRequest<GetPermissionByIdResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
    }
}
