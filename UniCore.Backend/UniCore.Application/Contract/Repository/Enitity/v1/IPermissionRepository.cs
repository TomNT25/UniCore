using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<Permission?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Permission?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
        Task<List<Permission>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        Task<List<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
    }
}