using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Admin.UserManagement.GetAllUsers
{
    public class UserListItemDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? StudentCode { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Gender { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public List<RoleDTO> Roles { get; set; } = new List<RoleDTO>();
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
