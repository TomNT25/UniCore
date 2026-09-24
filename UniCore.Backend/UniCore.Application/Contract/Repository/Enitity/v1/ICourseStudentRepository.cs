using System.Linq.Expressions;
using UniCore.Application.DTO;
using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface ICourseStudentRepository : IRepository<CourseStudent>
    {
        Task<IEnumerable<CourseStudent>?> GetByStuIdCourseIdsAsync(string stuId, IEnumerable<string> courseIds, CancellationToken ct);
        Task<IEnumerable<string>?> GetCourseIdsByStuIdAsync(string stuId,CancellationToken ct);
        Task<List<string>> GetActiveStudentIdsByCourseIdAsync(string courseId, CancellationToken cancellationToken = default);
	
        Task<PageNumberPaginationResponse<CourseStudent>> GetPaginatedByStuIdCourseIdsAsync(string stuId, 
            PageNumberPaginationRequest pgRequest,
            CancellationToken cancellationToken = default,
            Expression<Func<CourseStudent, bool>>? filter = null
            );
        Task<CourseStudent?> GetDetailedStuIdCourseIdsAsync(string studentId,
            string courseId, 
            CancellationToken ct);
    }
}
