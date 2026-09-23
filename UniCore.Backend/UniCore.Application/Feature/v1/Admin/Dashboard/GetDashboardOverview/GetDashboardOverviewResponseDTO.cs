using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Admin.Dashboard.GetDashboardOverview
{
    public class GetDashboardOverviewResponseDTO
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int VerifiedUsers { get; set; }
        public int TotalRoles { get; set; }
        public int TotalDepartments { get; set; }
        public int TotalCourses { get; set; }
        public List<UserDTO> RecentUsers { get; set; } = new List<UserDTO>();
    }
}
