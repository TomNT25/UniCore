using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UniCore.Application.Contract.UnitOfWork;

namespace UniCore.Infrastructure.UnitOfWork
{
    public class UnitOfWorkTransaction : IUnitOfWorkTransaction
    {
        private readonly IDbContextTransaction? _dbTransaction;
        private readonly DbContext _dbContext;
        private readonly Action _onDisposeCallback;

        public bool IsOuter { get; }
        public string? SavepointName { get; }
        public bool IsCompleted { get; private set; }

        public UnitOfWorkTransaction(
            DbContext dbContext,
            IDbContextTransaction? dbTransaction,
            bool isOuter,
            string? savepointName,
            Action onDisposeCallback)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbTransaction = dbTransaction;
            IsOuter = isOuter;
            SavepointName = savepointName;
            _onDisposeCallback = onDisposeCallback ?? throw new ArgumentNullException(nameof(onDisposeCallback));
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (IsCompleted)
                return;

            await _dbContext.SaveChangesAsync(cancellationToken);

            if (IsOuter && _dbTransaction != null)
            {
                await _dbTransaction.CommitAsync(cancellationToken);
            }

            IsCompleted = true;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (IsCompleted)
                return;

            if (IsOuter)
            {
                if (_dbTransaction != null)
                {
                    await _dbTransaction.RollbackAsync(cancellationToken);
                }
            }
            else if (!string.IsNullOrEmpty(SavepointName) && _dbTransaction != null)
            {
                await _dbTransaction.RollbackToSavepointAsync(SavepointName, cancellationToken);
            }

            IsCompleted = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (IsOuter && _dbTransaction != null)
                {
                    _dbTransaction.Dispose();
                }
                _onDisposeCallback();
            }
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (IsOuter && _dbTransaction != null)
            {
                await _dbTransaction.DisposeAsync();
            }
            _onDisposeCallback();
        }
    }
}
