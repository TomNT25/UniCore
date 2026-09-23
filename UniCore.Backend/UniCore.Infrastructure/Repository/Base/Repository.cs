using UniCore.Application.Contract.Repository;
using UniCore.Application.DTO;
using System.Linq.Expressions;

namespace UniCore.Infrastructure.Repository.Base
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        public abstract Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        public abstract Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default);
        public abstract Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        public abstract Task<T?> GetByIDAsync(int id, CancellationToken cancellationToken = default);

        public virtual Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual Task<PageNumberPaginationResponse<TDto>> GetPageNumberPaginationAsync<TDto>(PageNumberPaginationRequest request, Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual Task<CursorPaginationResponse<TDto>> GetCursorPaginationAsync<TDto>(CursorPaginationRequest request, Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public abstract Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public abstract Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
