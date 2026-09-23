using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

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
                            .ToListAsync();

            return result;
        }
    }
}
