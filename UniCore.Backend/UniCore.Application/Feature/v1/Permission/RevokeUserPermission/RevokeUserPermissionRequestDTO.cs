using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Permission.RevokeUserPermission
{
    public class RevokeUserPermissionRequestDTO : IRequest<RevokeUserPermissionResponseDTO>
    {
        public string UserId { get; set; } = string.Empty;
        public List<string> PermissionIds { get; set; } = new();
    }
}
