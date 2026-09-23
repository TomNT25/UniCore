using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Permission.GrantUserPermission
{
    public class GrantUserPermissionRequestDTO : IRequest<GrantUserPermissionResponseDTO>
    {
        public string UserId { get; set; } = string.Empty;
        public List<string> PermissionIds { get; set; } = new();
    }
}
