using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Role.GrantRolePermission
{
    public class GrantRolePermissionRequestDTO : IRequest<GrantRolePermissionResponseDTO>
    {
        public string RoleId { get; set; } = string.Empty;
        public List<string> PermissionIds { get; set; } = new();
    }
}
