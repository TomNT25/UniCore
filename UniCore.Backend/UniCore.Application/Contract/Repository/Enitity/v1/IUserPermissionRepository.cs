using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IUserPermissionRepository : IRepository<UserPermission>
    {
        Task<IEnumerable<UserPermission>> GetByUserIDAsync(string userId, CancellationToken cancellationToken = default);
        Task<List<UserPermission>> GetByUserAndPermissionIdsAsync(string userId, IEnumerable<string> permissionIds, CancellationToken cancellationToken = default);
    }
}

