using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>?> GetCourseInfosByIds(IEnumerable<string> courseIds, CancellationToken ct = default);
        Task<Course?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Course?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    }
}
