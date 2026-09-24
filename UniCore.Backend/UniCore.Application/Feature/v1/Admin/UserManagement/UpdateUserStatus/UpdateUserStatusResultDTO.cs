namespace UniCore.Application.Feature.v1.Admin.UserManagement.UpdateUserStatus
{
    public class UpdateUserStatusResultDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
