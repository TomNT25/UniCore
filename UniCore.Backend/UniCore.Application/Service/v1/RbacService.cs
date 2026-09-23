using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.Permission.GrantUserPermission;
using UniCore.Application.Feature.v1.Permission.RevokeUserPermission;
using UniCore.Application.Feature.v1.Rbac.GetRbacOverview;
using UniCore.Application.Feature.v1.Rbac.GetUserAccessMatrix;
using UniCore.Application.Feature.v1.Role.GrantRolePermission;
using UniCore.Application.Feature.v1.Role.GrantUserRole;
using UniCore.Application.Feature.v1.Role.RevokeRolePermission;
using UniCore.Application.Feature.v1.Role.RevokeUserRole;

namespace UniCore.Application.Service.v1
{
    public class RbacService : IRbacService
    {
        private readonly GetRbacOverviewHandler _getRbacOverviewHandler;
        private readonly GetUserAccessMatrixHandler _getUserAccessMatrixHandler;
        private readonly GrantUserRoleHandler _grantUserRoleHandler;
        private readonly RevokeUserRoleHandler _revokeUserRoleHandler;
        private readonly GrantUserPermissionHandler _grantUserPermissionHandler;
        private readonly RevokeUserPermissionHandler _revokeUserPermissionHandler;
        private readonly GrantRolePermissionHandler _grantRolePermissionHandler;
        private readonly RevokeRolePermissionHandler _revokeRolePermissionHandler;

        public RbacService(
            GetRbacOverviewHandler getRbacOverviewHandler,
            GetUserAccessMatrixHandler getUserAccessMatrixHandler,
            GrantUserRoleHandler grantUserRoleHandler,
            RevokeUserRoleHandler revokeUserRoleHandler,
            GrantUserPermissionHandler grantUserPermissionHandler,
            RevokeUserPermissionHandler revokeUserPermissionHandler,
            GrantRolePermissionHandler grantRolePermissionHandler,
            RevokeRolePermissionHandler revokeRolePermissionHandler)
        {
            _getRbacOverviewHandler = getRbacOverviewHandler;
            _getUserAccessMatrixHandler = getUserAccessMatrixHandler;
            _grantUserRoleHandler = grantUserRoleHandler;
            _revokeUserRoleHandler = revokeUserRoleHandler;
            _grantUserPermissionHandler = grantUserPermissionHandler;
            _revokeUserPermissionHandler = revokeUserPermissionHandler;
            _grantRolePermissionHandler = grantRolePermissionHandler;
            _revokeRolePermissionHandler = revokeRolePermissionHandler;
        }

        public Task<GetRbacOverviewResponseDTO> GetOverviewAsync(GetRbacOverviewRequestDTO request, CancellationToken cancellationToken = default)
            => _getRbacOverviewHandler.HandleAsync(request, cancellationToken);

        public Task<GetUserAccessMatrixResponseDTO> GetUserAccessMatrixAsync(GetUserAccessMatrixRequestDTO request, CancellationToken cancellationToken = default)
            => _getUserAccessMatrixHandler.HandleAsync(request, cancellationToken);

        public Task<GrantUserRoleResponseDTO> GrantUserRoleAsync(GrantUserRoleRequestDTO request, CancellationToken cancellationToken = default)
            => _grantUserRoleHandler.HandleAsync(request, cancellationToken);

        public Task<RevokeUserRoleResponseDTO> RevokeUserRoleAsync(RevokeUserRoleRequestDTO request, CancellationToken cancellationToken = default)
            => _revokeUserRoleHandler.HandleAsync(request, cancellationToken);

        public Task<GrantUserPermissionResponseDTO> GrantUserPermissionAsync(GrantUserPermissionRequestDTO request, CancellationToken cancellationToken = default)
            => _grantUserPermissionHandler.HandleAsync(request, cancellationToken);

        public Task<RevokeUserPermissionResponseDTO> RevokeUserPermissionAsync(RevokeUserPermissionRequestDTO request, CancellationToken cancellationToken = default)
            => _revokeUserPermissionHandler.HandleAsync(request, cancellationToken);

        public Task<GrantRolePermissionResponseDTO> GrantRolePermissionAsync(GrantRolePermissionRequestDTO request, CancellationToken cancellationToken = default)
            => _grantRolePermissionHandler.HandleAsync(request, cancellationToken);

        public Task<RevokeRolePermissionResponseDTO> RevokeRolePermissionAsync(RevokeRolePermissionRequestDTO request, CancellationToken cancellationToken = default)
            => _revokeRolePermissionHandler.HandleAsync(request, cancellationToken);
    }
}
