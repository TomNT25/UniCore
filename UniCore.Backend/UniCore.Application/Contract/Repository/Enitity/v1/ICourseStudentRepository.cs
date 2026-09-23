using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface ICourseStudentRepository : IRepository<CourseStudent>
    {
        Task<IEnumerable<CourseStudent>?> GetByStuIdCourseIdsAsync(string stuId, IEnumerable<string> courseIds,CancellationToken ct);
    }
}
