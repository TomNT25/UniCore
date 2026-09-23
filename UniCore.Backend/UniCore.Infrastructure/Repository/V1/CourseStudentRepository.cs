using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;
using static UniCore.Helper.Constant.MessageConstants;

namespace UniCore.Infrastructure.Repository.V1
{
    public class CourseStudentRepository : RepositoryEFCoreBase<CourseStudent>, ICourseStudentRepository
    {
        public CourseStudentRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<IEnumerable<CourseStudent>?> GetByStuIdCourseIdsAsync(string studentId, IEnumerable<string> courseIds, CancellationToken ct)
        {
            var result = await _dbSet
                            .Where(x => x.UserId.Equals(studentId) && courseIds.Contains(x.CourseId))
                            .Take(30)
                            .AsNoTracking()
                            .ToListAsync(ct);

            return result;
        }

        public async Task<IEnumerable<string>?> GetCourseIdsByStuIdAsync(string stuId, CancellationToken ct)
        {
            var results = await _dbSet
                .Where(x => x.UserId.Equals(stuId))
                .Select(x => x.CourseId)
                .Take(30)
                .AsNoTracking()
                .ToListAsync(ct);

            return results;
        }

        public async Task<List<string>> GetActiveStudentIdsByCourseIdAsync(string courseId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(cs =>
                    cs.CourseId == courseId &&
                    cs.IsActive &&
                    !cs.IsDeleted)
                .Select(cs => cs.User.StudentCode)
                .Distinct()
                .ToListAsync(cancellationToken);
        }
    }
}
