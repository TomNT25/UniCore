using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Role.GrantUserRole
{
    public class GrantUserRoleRequestDTO : IRequest<GrantUserRoleResponseDTO>
    {
        public string UserId { get; set; } = string.Empty;
        public List<string> RoleIds { get; set; } = new();
    }
}
