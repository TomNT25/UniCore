using MapsterMapper;
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

        public async Task<IEnumerable<Course>?> GetCourseInfosByIdsAsync(IEnumerable<string> courseIds, CancellationToken ct = default)
        {
            var results = await _dbSet
                                .Where(x => courseIds.Contains(x.Id))
                                .Take(30)
                                .AsNoTracking()
                                .ToListAsync(ct);

            return results;
        }

        public async Task<Course?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking()
                                .Include(c => c.Department)
                                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
        }

        public async Task<Course?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.Department)
                .FirstOrDefaultAsync(c => c.Code == code && !c.IsDeleted, cancellationToken);
        }

        public async Task<(List<Course> Items, int TotalCount)> SearchActiveAsync(
            string? search,
            int limit,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsNoTracking().Where(c => c.IsActive && !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(c =>
                    EF.Functions.Like(c.Name, $"%{term}%") ||
                    (c.Code != null && EF.Functions.Like(c.Code, $"%{term}%")));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderBy(c => c.Name)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
