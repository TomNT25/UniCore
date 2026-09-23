using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Department?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<(List<Department> Items, int TotalCount)> SearchActiveAsync(
            string? search,
            int limit,
            CancellationToken cancellationToken = default);
    }
}
