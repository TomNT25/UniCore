namespace UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUser
{
    public class UpdateUserResultDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
