using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Feature.v1.Permission.CreatePermission;
using UniCore.Application.Feature.v1.Permission.DeletePermission;
using UniCore.Application.Feature.v1.Permission.GetAllPermission;
using UniCore.Application.Feature.v1.Permission.GetPermissionById;
using UniCore.Application.Feature.v1.Permission.GrantUserPermission;
using UniCore.Application.Feature.v1.Permission.RevokeUserPermission;
using UniCore.Application.Feature.v1.Permission.UpdatePermission;

namespace UniCore.Application.Service.v1
{
    public class PermissionService : IPermissionService
    {
        private readonly GetAllPermissionHandler _getAllPermissionHandler;
        private readonly GetPermissionByIdHandler _getPermissionByIdHandler;
        private readonly CreatePermissionHandler _createPermissionHandler;
        private readonly UpdatePermissionHandler _updatePermissionHandler;
        private readonly DeletePermissionHandler _deletePermissionHandler;
        private readonly GrantUserPermissionHandler _grantUserPermissionHandler;
        private readonly RevokeUserPermissionHandler _revokeUserPermissionHandler;

        public PermissionService(
            GetAllPermissionHandler getAllPermissionHandler,
            GetPermissionByIdHandler getPermissionByIdHandler,
            CreatePermissionHandler createPermissionHandler,
            UpdatePermissionHandler updatePermissionHandler,
            DeletePermissionHandler deletePermissionHandler,
            GrantUserPermissionHandler grantUserPermissionHandler,
            RevokeUserPermissionHandler revokeUserPermissionHandler)
        {
            _getAllPermissionHandler = getAllPermissionHandler;
            _getPermissionByIdHandler = getPermissionByIdHandler;
            _createPermissionHandler = createPermissionHandler;
            _updatePermissionHandler = updatePermissionHandler;
            _deletePermissionHandler = deletePermissionHandler;
            _grantUserPermissionHandler = grantUserPermissionHandler;
            _revokeUserPermissionHandler = revokeUserPermissionHandler;
        }

        public Task<GetAllPermissionResponseDTO> GetAllAsync(GetAllPermissionRequestDTO request, CancellationToken cancellationToken = default)
            => _getAllPermissionHandler.HandleAsync(request, cancellationToken);

        public Task<GetPermissionByIdResponseDTO> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => _getPermissionByIdHandler.HandleAsync(new GetPermissionByIdRequestDTO { Id = id }, cancellationToken);

        public Task<CreatePermissionResponseDTO> CreateAsync(CreatePermissionRequestDTO request, CancellationToken cancellationToken = default)
            => _createPermissionHandler.HandleAsync(request, cancellationToken);

        public Task<UpdatePermissionResponseDTO> UpdateAsync(UpdatePermissionRequestDTO request, CancellationToken cancellationToken = default)
            => _updatePermissionHandler.HandleAsync(request, cancellationToken);

        public Task<DeletePermissionResponseDTO> DeleteAsync(string id, CancellationToken cancellationToken = default)
            => _deletePermissionHandler.HandleAsync(new DeletePermissionRequestDTO { Id = id }, cancellationToken);

        public Task<GrantUserPermissionResponseDTO> GrantUserPermissionAsync(GrantUserPermissionRequestDTO request, CancellationToken cancellationToken = default)
            => _grantUserPermissionHandler.HandleAsync(request, cancellationToken);

        public Task<RevokeUserPermissionResponseDTO> RevokeUserPermissionAsync(RevokeUserPermissionRequestDTO request, CancellationToken cancellationToken = default)
            => _revokeUserPermissionHandler.HandleAsync(request, cancellationToken);
    }
}
