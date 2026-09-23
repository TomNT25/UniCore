using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Permission.DeletePermission
{
    public class DeletePermissionRequestDTO : IRequest<DeletePermissionResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
    }
}
