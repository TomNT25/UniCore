using UniCore.Application.Feature.v1.Permission.GrantUserPermission;
using UniCore.Application.Feature.v1.Permission.RevokeUserPermission;
using UniCore.Application.Feature.v1.Rbac.GetRbacOverview;
using UniCore.Application.Feature.v1.Rbac.GetUserAccessMatrix;
using UniCore.Application.Feature.v1.Role.GrantRolePermission;
using UniCore.Application.Feature.v1.Role.GrantUserRole;
using UniCore.Application.Feature.v1.Role.RevokeRolePermission;
using UniCore.Application.Feature.v1.Role.RevokeUserRole;

namespace UniCore.Application.Contract.Service.v1
{
    public interface IRbacService
    {
        Task<GetRbacOverviewResponseDTO> GetOverviewAsync(GetRbacOverviewRequestDTO request, CancellationToken cancellationToken = default);
        Task<GetUserAccessMatrixResponseDTO> GetUserAccessMatrixAsync(GetUserAccessMatrixRequestDTO request, CancellationToken cancellationToken = default);
        Task<GrantUserRoleResponseDTO> GrantUserRoleAsync(GrantUserRoleRequestDTO request, CancellationToken cancellationToken = default);
        Task<RevokeUserRoleResponseDTO> RevokeUserRoleAsync(RevokeUserRoleRequestDTO request, CancellationToken cancellationToken = default);
        Task<GrantUserPermissionResponseDTO> GrantUserPermissionAsync(GrantUserPermissionRequestDTO request, CancellationToken cancellationToken = default);
        Task<RevokeUserPermissionResponseDTO> RevokeUserPermissionAsync(RevokeUserPermissionRequestDTO request, CancellationToken cancellationToken = default);
        Task<GrantRolePermissionResponseDTO> GrantRolePermissionAsync(GrantRolePermissionRequestDTO request, CancellationToken cancellationToken = default);
        Task<RevokeRolePermissionResponseDTO> RevokeRolePermissionAsync(RevokeRolePermissionRequestDTO request, CancellationToken cancellationToken = default);
    }
}
