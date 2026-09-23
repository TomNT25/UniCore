using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Role?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
        Task<Role?> GetDefaultRoleAsync(CancellationToken cancellationToken = default);
    }
}

