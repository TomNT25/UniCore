using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class ClassCourseRepository : RepositoryEFCoreBase<ClassCourse>, IClassCourseRepository
    {
        public ClassCourseRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<IEnumerable<string>?> GetCourseIdsByClassIdAsync(string classId, CancellationToken ct = default)
        {
            var results = await _dbSet
                .Where(x => x.SchoolClassId.Equals(classId))
                .Select(x => x.CourseId)
                .Take(30)
                .AsNoTracking()
                .ToListAsync(ct);
            return results;
        }
    }
}
