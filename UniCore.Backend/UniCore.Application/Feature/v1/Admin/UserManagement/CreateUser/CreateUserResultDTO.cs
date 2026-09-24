namespace UniCore.Application.Feature.v1.Admin.UserManagement.CreateUser
{
    public class CreateUserResultDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string? StudentCode { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
