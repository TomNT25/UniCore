namespace UniCore.Application.Feature.v1.Admin.Dashboard.GetDashboardOverview
{
    public class DashboardRecentUserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public List<string> RoleNames { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
