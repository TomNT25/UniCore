using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UniCore.Application.Contract.Repository;
using UniCore.Application.Contract.UnitOfWork;
using UniCore.Application.DTO;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Extension;

namespace UniCore.Infrastructure.Repository.Base
{
    public abstract class RepositoryEFCoreBase<T> : IRepository<T> where T : class
    {
        protected readonly UniCoreDbContext _UniCoreDbContext;
        protected readonly DbSet<T> _dbSet;
        protected readonly IMapper _mapper;

        public RepositoryEFCoreBase(
            UniCoreDbContext UniCoreDbcontext,
            IMapper mapper)
        {
            _UniCoreDbContext = UniCoreDbcontext;
            _dbSet = _UniCoreDbContext.Set<T>();
            _mapper = mapper;
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            await _UniCoreDbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
            await _UniCoreDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            await _UniCoreDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            _UniCoreDbContext.SaveChanges();
        }

        public async Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIDAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().AnyAsync(predicate, cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            return predicate != null
                ? await _dbSet.AsNoTracking().CountAsync(predicate, cancellationToken)
                : await _dbSet.AsNoTracking().CountAsync(cancellationToken);
        }

        public async Task<PageNumberPaginationResponse<TDto>> GetPageNumberPaginationAsync<TDto>(
            PageNumberPaginationRequest request,
            Expression<Func<T, bool>>? filter = null,
            CancellationToken cancellationToken = default
        )
        {
            var query = _dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalRecords = await query.CountAsync(cancellationToken);

            var sortCol = string.IsNullOrWhiteSpace(request.SortColumn) ? "Id" : request.SortColumn;

            query = query.OrderByDynamic(sortCol, request.SortDescending);

            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;
            var skip = (pageNumber - 1) * pageSize;

            var items = await query
                .Skip(skip)
                .Take(pageSize)
                .ProjectToType<TDto>()
                .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var result = new PageNumberPaginationResponse<TDto>()
            {
                Items = items,
                Metadata = new PageNumberPaginationMetaResponse
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    HasNextPage = pageNumber < totalPages,
                    HasPreviousPage = pageNumber > 1
                }
            };

            return result;
        }

        public async Task<CursorPaginationResponse<TDto>> GetCursorPaginationAsync<TDto>(
            CursorPaginationRequest request,
            Expression<Func<T, bool>>? filter = null,
            CancellationToken cancellationToken = default
        )
        {
            var query = _dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var sortCol = string.IsNullOrWhiteSpace(request.SortColumn) ? "Id" : request.SortColumn;

            query = query.ApplyCursorFilter(request.Cursor, sortCol, request.SortDescending);
            query = query.OrderByDynamic(sortCol, request.SortDescending);

            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

            var items = await query
                .Take(pageSize + 1)
                .ProjectToType<TDto>()
                .ToListAsync(cancellationToken);

            var hasNextPage = items.Count > pageSize;
            if (hasNextPage)
            {
                items.RemoveAt(items.Count - 1);
            }

            string? nextCursor = null;
            if (hasNextPage && items.Count > 0)
            {
                var lastItem = items[items.Count - 1];
                var normalizedSort = sortCol.Replace("_", "");
                var prop = typeof(TDto).GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, sortCol, StringComparison.OrdinalIgnoreCase) ||
                                         string.Equals(p.Name, normalizedSort, StringComparison.OrdinalIgnoreCase))
                    ?? typeof(TDto).GetProperties()
                        .FirstOrDefault(p => string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase) ||
                                             p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase));

                if (prop != null)
                {
                    var rawVal = prop.GetValue(lastItem)?.ToString();
                    if (!string.IsNullOrEmpty(rawVal))
                    {
                        nextCursor = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(rawVal));
                    }
                }
            }

            return new CursorPaginationResponse<TDto>()
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

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _UniCoreDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _UniCoreDbContext.Entry(entity).State = EntityState.Modified;
            await _UniCoreDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var currentDbTx = _UniCoreDbContext.Database.CurrentTransaction;

            if (currentDbTx == null)
            {
                var dbTx = await _UniCoreDbContext.Database.BeginTransactionAsync(cancellationToken);
                return new UnitOfWork.UnitOfWorkTransaction(_UniCoreDbContext, dbTx, isOuter: true, savepointName: null, onDisposeCallback: () => { });
            }
            else
            {
                var savepointName = $"sp_{Guid.NewGuid():N}";
                await currentDbTx.CreateSavepointAsync(savepointName, cancellationToken);
                return new UnitOfWork.UnitOfWorkTransaction(_UniCoreDbContext, currentDbTx, isOuter: false, savepointName: savepointName, onDisposeCallback: () => { });
            }
        }
    }
}
