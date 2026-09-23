using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Rbac.GetRbacOverview
{
    public class GetRbacOverviewRequestDTO : IRequest<GetRbacOverviewResponseDTO>
    {
    }

    public class RoleOverviewDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int AssignedPermissionsCount { get; set; }
        public int AssignedUsersCount { get; set; }
    }

    public class GetRbacOverviewResponseDTO
    {
        public int TotalUsers { get; set; }
        public int TotalRoles { get; set; }
        public int TotalPermissions { get; set; }
        public int TotalUserRoles { get; set; }
        public int TotalUserPermissions { get; set; }
        public int TotalRolePermissions { get; set; }
        public List<RoleOverviewDTO> RolesOverview { get; set; } = new();
    }
}
