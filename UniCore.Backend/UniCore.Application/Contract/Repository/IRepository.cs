using System.Linq.Expressions;
using UniCore.Application.DTO;

namespace UniCore.Application.Contract.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task<T?> GetByIDAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

        Task<PageNumberPaginationResponse<TDto>> GetPageNumberPaginationAsync<TDto>(
            PageNumberPaginationRequest request,
            Expression<Func<T, bool>>? filter = null,
            CancellationToken cancellationToken = default);

        Task<CursorPaginationResponse<TDto>> GetCursorPaginationAsync<TDto>(
            CursorPaginationRequest request,
            Expression<Func<T, bool>>? filter = null,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default);
        void DeleteRange(IEnumerable<T> entities);
        Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
