using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task <IEnumerable<Course>?> GetCourseInfosByIdsAsync(IEnumerable<string> courseIds, CancellationToken ct = default);
        
        Task<Course?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<(List<Course> Items, int TotalCount)> SearchActiveAsync(
            string? search,
            int limit,
            CancellationToken cancellationToken = default);
        Task<Course?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    }
}