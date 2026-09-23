using System;
using System.Threading;
using System.Threading.Tasks;
using UniCore.Application.Contract.Repository;

namespace UniCore.Application.Contract.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether there is an active physical database transaction.
        /// </summary>
        bool HasActiveTransaction { get; }

        /// <summary>
        /// Gets the current active transaction scope if any exists.
        /// </summary>
        IUnitOfWorkTransaction? CurrentTransaction { get; }

        /// <summary>
        /// Saves all pending entity state changes tracked by the DbContext.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins a new transaction scope.
        /// If an active outer transaction exists, a named Savepoint is created (IsOuter = false).
        /// Otherwise, a physical database transaction is started (IsOuter = true).
        /// </summary>
        Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits the active transaction scope.
        /// </summary>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the active transaction scope (or savepoint).
        /// </summary>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a repository instance for the specified entity type.
        /// </summary>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}
