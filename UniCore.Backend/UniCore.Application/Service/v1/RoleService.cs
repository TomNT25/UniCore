using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.Role.CreateRole;
using UniCore.Application.Feature.v1.Role.DeleteRole;
using UniCore.Application.Feature.v1.Role.GetAllRole;
using UniCore.Application.Feature.v1.Role.GetRoleById;
using UniCore.Application.Feature.v1.Role.GrantRolePermission;
using UniCore.Application.Feature.v1.Role.GrantUserRole;
using UniCore.Application.Feature.v1.Role.RevokeRolePermission;
using UniCore.Application.Feature.v1.Role.RevokeUserRole;
using UniCore.Application.Feature.v1.Role.UpdateRole;

namespace UniCore.Application.Service.v1
{
    public class RoleService : IRoleService
    {
        private readonly GetAllRoleHandler _getAllRoleHandler;
        private readonly GetRoleByIdHandler _getRoleByIdHandler;
        private readonly CreateRoleHandler _createRoleHandler;
        private readonly UpdateRoleHandler _updateRoleHandler;
        private readonly DeleteRoleHandler _deleteRoleHandler;
        private readonly GrantRolePermissionHandler _grantRolePermissionHandler;
        private readonly RevokeRolePermissionHandler _revokeRolePermissionHandler;
        private readonly GrantUserRoleHandler _grantUserRoleHandler;
        private readonly RevokeUserRoleHandler _revokeUserRoleHandler;

        public RoleService(
            GetAllRoleHandler getAllRoleHandler,
            GetRoleByIdHandler getRoleByIdHandler,
            CreateRoleHandler createRoleHandler,
            UpdateRoleHandler updateRoleHandler,
            DeleteRoleHandler deleteRoleHandler,
            GrantRolePermissionHandler grantRolePermissionHandler,
            RevokeRolePermissionHandler revokeRolePermissionHandler,
            GrantUserRoleHandler grantUserRoleHandler,
            RevokeUserRoleHandler revokeUserRoleHandler)
        {
            _getAllRoleHandler = getAllRoleHandler;
            _getRoleByIdHandler = getRoleByIdHandler;
            _createRoleHandler = createRoleHandler;
            _updateRoleHandler = updateRoleHandler;
            _deleteRoleHandler = deleteRoleHandler;
            _grantRolePermissionHandler = grantRolePermissionHandler;
            _revokeRolePermissionHandler = revokeRolePermissionHandler;
            _grantUserRoleHandler = grantUserRoleHandler;
            _revokeUserRoleHandler = revokeUserRoleHandler;
        }

        public Task<GetAllRoleResponseDTO> GetAllAsync(GetAllRoleRequestDTO request, CancellationToken cancellationToken = default)
            => _getAllRoleHandler.HandleAsync(request, cancellationToken);

        public Task<GetRoleByIdResponseDTO> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => _getRoleByIdHandler.HandleAsync(new GetRoleByIdRequestDTO { Id = id }, cancellationToken);

        public Task<CreateRoleResponseDTO> CreateAsync(CreateRoleRequestDTO request, CancellationToken cancellationToken = default)
            => _createRoleHandler.HandleAsync(request, cancellationToken);

        public Task<UpdateRoleResponseDTO> UpdateAsync(UpdateRoleRequestDTO request, CancellationToken cancellationToken = default)
            => _updateRoleHandler.HandleAsync(request, cancellationToken);

        public Task<DeleteRoleResponseDTO> DeleteAsync(string id, CancellationToken cancellationToken = default)
            => _deleteRoleHandler.HandleAsync(new DeleteRoleRequestDTO { Id = id }, cancellationToken);

        public Task<GrantRolePermissionResponseDTO> GrantRolePermissionAsync(GrantRolePermissionRequestDTO request, CancellationToken cancellationToken = default)
            => _grantRolePermissionHandler.HandleAsync(request, cancellationToken);

        public Task<RevokeRolePermissionResponseDTO> RevokeRolePermissionAsync(RevokeRolePermissionRequestDTO request, CancellationToken cancellationToken = default)
            => _revokeRolePermissionHandler.HandleAsync(request, cancellationToken);

        public Task<GrantUserRoleResponseDTO> GrantUserRoleAsync(GrantUserRoleRequestDTO request, CancellationToken cancellationToken = default)
            => _grantUserRoleHandler.HandleAsync(request, cancellationToken);

        public Task<RevokeUserRoleResponseDTO> RevokeUserRoleAsync(RevokeUserRoleRequestDTO request, CancellationToken cancellationToken = default)
            => _revokeUserRoleHandler.HandleAsync(request, cancellationToken);
    }
}
