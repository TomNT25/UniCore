using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Role.RevokeUserRole
{
    public class RevokeUserRoleRequestDTO : IRequest<RevokeUserRoleResponseDTO>
    {
        public string UserId { get; set; } = string.Empty;
        public List<string> RoleIds { get; set; } = new();
    }
}
