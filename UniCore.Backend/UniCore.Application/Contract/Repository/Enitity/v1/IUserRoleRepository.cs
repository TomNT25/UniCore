using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Task<List<UserRole>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<List<UserRole>> GetByUserAndRoleIdsAsync(string userId, IEnumerable<string> roleIds, CancellationToken cancellationToken = default);
    }
}
