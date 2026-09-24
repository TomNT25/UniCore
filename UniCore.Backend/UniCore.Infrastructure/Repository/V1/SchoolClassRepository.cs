using System.Text;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.DTO;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Extension;
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

        public async Task<CursorPaginationResponse<SchoolClass>> SearchActiveCursorAsync(
            CursorPaginationRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsNoTracking().Where(c => c.IsActive && !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim();
                query = query.Where(c =>
                    EF.Functions.Like(c.Name, $"%{term}%") ||
                    (c.Code != null && EF.Functions.Like(c.Code, $"%{term}%")));
            }

            var sortCol = string.IsNullOrWhiteSpace(request.SortColumn) ? "Id" : request.SortColumn;
            query = query.ApplyCursorFilter(request.Cursor, sortCol, request.SortDescending);
            query = query.OrderByDynamic(sortCol, request.SortDescending);

            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

            var items = await query
                .Take(pageSize + 1)
                .ToListAsync(cancellationToken);

            var hasNextPage = items.Count > pageSize;
            if (hasNextPage)
            {
                items.RemoveAt(items.Count - 1);
            }

            string? nextCursor = null;
            if (hasNextPage && items.Count > 0)
            {
                var lastItem = items[^1];
                var normalizedSort = sortCol.Replace("_", "");
                var prop = typeof(SchoolClass).GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, sortCol, StringComparison.OrdinalIgnoreCase) ||
                                         string.Equals(p.Name, normalizedSort, StringComparison.OrdinalIgnoreCase))
                    ?? typeof(SchoolClass).GetProperties()
                        .FirstOrDefault(p => string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase) ||
                                             p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase));

                if (prop != null)
                {
                    var rawVal = prop.GetValue(lastItem)?.ToString();
                    if (!string.IsNullOrEmpty(rawVal))
                    {
                        nextCursor = Convert.ToBase64String(Encoding.UTF8.GetBytes(rawVal));
                    }
                }
            }

            return new CursorPaginationResponse<SchoolClass>
            {
                Items = items,
                Metadata = new CursorPaginationMetaResponse
                {
                    PageSize = pageSize,
                    HasNextPage = hasNextPage,
                    NextCursor = nextCursor
                }
            };
        }
    }
}
