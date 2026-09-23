using UniCore.Application.Feature.v1.Role.CreateRole;
using UniCore.Application.Feature.v1.Role.DeleteRole;
using UniCore.Application.Feature.v1.Role.GetAllRole;
using UniCore.Application.Feature.v1.Role.GetRoleById;
using UniCore.Application.Feature.v1.Role.GrantRolePermission;
using UniCore.Application.Feature.v1.Role.GrantUserRole;
using UniCore.Application.Feature.v1.Role.RevokeRolePermission;
using UniCore.Application.Feature.v1.Role.RevokeUserRole;
using UniCore.Application.Feature.v1.Role.UpdateRole;

namespace UniCore.Application.Contract.Service.v1
{
    public interface IRoleService
    {
        Task<GetAllRoleResponseDTO> GetAllAsync(GetAllRoleRequestDTO request, CancellationToken cancellationToken = default);
        Task<GetRoleByIdResponseDTO> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<CreateRoleResponseDTO> CreateAsync(CreateRoleRequestDTO request, CancellationToken cancellationToken = default);
        Task<UpdateRoleResponseDTO> UpdateAsync(UpdateRoleRequestDTO request, CancellationToken cancellationToken = default);
        Task<DeleteRoleResponseDTO> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<GrantRolePermissionResponseDTO> GrantRolePermissionAsync(GrantRolePermissionRequestDTO request, CancellationToken cancellationToken = default);
        Task<RevokeRolePermissionResponseDTO> RevokeRolePermissionAsync(RevokeRolePermissionRequestDTO request, CancellationToken cancellationToken = default);
        Task<GrantUserRoleResponseDTO> GrantUserRoleAsync(GrantUserRoleRequestDTO request, CancellationToken cancellationToken = default);
        Task<RevokeUserRoleResponseDTO> RevokeUserRoleAsync(RevokeUserRoleRequestDTO request, CancellationToken cancellationToken = default);
    }
}
