using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Rbac.GetUserAccessMatrix
{
    public class GetUserAccessMatrixRequestDTO : IRequest<GetUserAccessMatrixResponseDTO>
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class UserRoleDetailDTO
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleCode { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string AssignmentType { get; set; } = string.Empty; // "Primary" or "Additional"
        public List<PermissionDTO> RolePermissions { get; set; } = new();
    }

    public class GetUserAccessMatrixResponseDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<UserRoleDetailDTO> Roles { get; set; } = new();
        public List<PermissionDTO> DirectUserPermissions { get; set; } = new();
        public List<PermissionDTO> EffectivePermissions { get; set; } = new();
    }
}
