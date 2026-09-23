using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IRolePermissionRepository : IRepository<RolePermission>
    {
        Task<List<RolePermission>> GetByRoleIdAsync(string roleId, CancellationToken cancellationToken = default);
        Task<List<RolePermission>> GetByRoleAndPermissionIdsAsync(string roleId, IEnumerable<string> permissionIds, CancellationToken cancellationToken = default);
    }
}
