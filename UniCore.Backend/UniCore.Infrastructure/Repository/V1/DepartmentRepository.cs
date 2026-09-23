using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class DepartmentRepository : RepositoryEFCoreBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<Department?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<(List<Department> Items, int TotalCount)> SearchActiveAsync(
            string? search,
            int limit,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsNoTracking().Where(d => d.IsActive && !d.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(d =>
                    EF.Functions.Like(d.Name, $"%{term}%") ||
                    (d.Code != null && EF.Functions.Like(d.Code, $"%{term}%")));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderBy(d => d.Name)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
