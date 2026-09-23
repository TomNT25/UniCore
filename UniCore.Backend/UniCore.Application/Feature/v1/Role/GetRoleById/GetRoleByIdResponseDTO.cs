using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Role.GetRoleById
{
    public class GetRoleByIdResponseDTO
    {
        public RoleDTO? Role { get; set; }
        public List<PermissionDTO> Permissions { get; set; } = new();
    }
}
