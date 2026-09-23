using UniCore.Application.Entity;

namespace UniCore.Application.DTO.Entity
{
    public class UserDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Code { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
        public int? AvatarMediaFileId { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string Provider { get; set; } = "system";
        public int FailedLoginAttempts { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsEmailVerified { get; set; } = false;
        public DateTime? EmailVerifiedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public List<RoleDTO> Roles { get; set; } = new List<RoleDTO>();
        public List<PermissionDTO> Permissions { get; set; } = new List<PermissionDTO>();
    }
}
