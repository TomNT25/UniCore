using System;
using System.Threading;
using System.Threading.Tasks;

namespace UniCore.Application.Contract.UnitOfWork
{
    public interface IUnitOfWorkTransaction : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this is the outermost physical database transaction.
        /// If false, this scope represents an inner Savepoint on an existing transaction.
        /// </summary>
        bool IsOuter { get; }

        /// <summary>
        /// Gets the name of the savepoint if this is an inner transaction scope (IsOuter == false).
        /// </summary>
        string? SavepointName { get; }

        /// <summary>
        /// Gets a value indicating whether the transaction scope has completed (committed or rolled back).
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Commits the transaction if outer, or passes through if inner savepoint.
        /// </summary>
        Task CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the physical transaction if outer, or rolls back to the savepoint if inner.
        /// </summary>
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
