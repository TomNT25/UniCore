using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface ISchoolClassRepository : IRepository<SchoolClass>
    {
        Task<SchoolClass?> GetInfoByIdAsync(string classId, CancellationToken ct = default);
        Task<SchoolClass?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<(List<SchoolClass> Items, int TotalCount)> SearchActiveAsync(
            string? search,
            int limit,
            CancellationToken cancellationToken = default);
    }
}
