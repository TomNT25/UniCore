using UniCore.Application.Feature.v1.Permission.CreatePermission;
using UniCore.Application.Feature.v1.Permission.DeletePermission;
using UniCore.Application.Feature.v1.Permission.GetAllPermission;
using UniCore.Application.Feature.v1.Permission.GetPermissionById;
using UniCore.Application.Feature.v1.Permission.GrantUserPermission;
using UniCore.Application.Feature.v1.Permission.RevokeUserPermission;
using UniCore.Application.Feature.v1.Permission.UpdatePermission;

namespace UniCore.Application.Contract.Service.v1
{
    public interface IPermissionService
    {
        Task<GetAllPermissionResponseDTO> GetAllAsync(GetAllPermissionRequestDTO request, CancellationToken cancellationToken = default);
        Task<GetPermissionByIdResponseDTO> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<CreatePermissionResponseDTO> CreateAsync(CreatePermissionRequestDTO request, CancellationToken cancellationToken = default);
        Task<UpdatePermissionResponseDTO> UpdateAsync(UpdatePermissionRequestDTO request, CancellationToken cancellationToken = default);
        Task<DeletePermissionResponseDTO> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<GrantUserPermissionResponseDTO> GrantUserPermissionAsync(GrantUserPermissionRequestDTO request, CancellationToken cancellationToken = default);
        Task<RevokeUserPermissionResponseDTO> RevokeUserPermissionAsync(RevokeUserPermissionRequestDTO request, CancellationToken cancellationToken = default);
    }
}
