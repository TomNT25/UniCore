using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class SchoolClassRepository : RepositoryEFCoreBase<SchoolClass>, ISchoolClassRepository
    {
        public SchoolClassRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<SchoolClass?> GetInfoByIdAsync(
           string classIds,
           CancellationToken ct = default
           )
        {
            var results = await _dbSet
                .Where(s => s.Id.Equals(classIds))
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);
            return results;
        }

        public async Task<SchoolClass?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<(List<SchoolClass> Items, int TotalCount)> SearchActiveAsync(
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
