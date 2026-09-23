using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class CourseRepository : RepositoryEFCoreBase<Course>, ICourseRepository
    {
        public CourseRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<IEnumerable<Course>?> GetCourseInfosByIds(IEnumerable<string> courseIds, CancellationToken ct = default)
        {
            var results = await _dbSet
                                .Where(x => courseIds.Contains(x.Id))
                                .Take(30)
                                .AsNoTracking()
                                .ToListAsync(ct);

            return results;
        }
    }
}
