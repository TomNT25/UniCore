using MapsterMapper;
using System.Collections.Concurrent;
using UniCore.Application.Contract.Repository;
using UniCore.Application.Contract.UnitOfWork;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UniCoreDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ConcurrentDictionary<Type, object> _repositories = new();
        private readonly Stack<IUnitOfWorkTransaction> _transactionStack = new();

        public UnitOfWork(UniCoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public bool HasActiveTransaction => _dbContext.Database.CurrentTransaction != null;

        public IUnitOfWorkTransaction? CurrentTransaction =>
            _transactionStack.Count > 0 ? _transactionStack.Peek() : null;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var currentDbTx = _dbContext.Database.CurrentTransaction;

            if (currentDbTx == null)
            {
                // IsOuter = true (Physical DB Transaction)
                var dbTx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                var outerTx = new UnitOfWorkTransaction(
                    _dbContext,
                    dbTx,
                    isOuter: true,
                    savepointName: null,
                    onDisposeCallback: () => PopTransactionScope());

                _transactionStack.Push(outerTx);
                return outerTx;
            }
            else
            {
                // IsOuter = false (Nested Savepoint)
                var savepointName = $"sp_{Guid.NewGuid():N}";
                await currentDbTx.CreateSavepointAsync(savepointName, cancellationToken);

                var savepointTx = new UnitOfWorkTransaction(
                    _dbContext,
                    currentDbTx,
                    isOuter: false,
                    savepointName: savepointName,
                    onDisposeCallback: () => PopTransactionScope());

                _transactionStack.Push(savepointTx);
                return savepointTx;
            }
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (CurrentTransaction != null)
            {
                await CurrentTransaction.CommitAsync(cancellationToken);
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (CurrentTransaction != null)
            {
                await CurrentTransaction.RollbackAsync(cancellationToken);
            }
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            return (IRepository<TEntity>)_repositories.GetOrAdd(type, _ => new GenericRepositoryImplementation<TEntity>(_dbContext, _mapper));
        }

        private void PopTransactionScope()
        {
            if (_transactionStack.Count > 0)
            {
                _transactionStack.Pop();
            }
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
                while (_transactionStack.Count > 0)
                {
                    var tx = _transactionStack.Pop();
                    tx.Dispose();
                }
            }
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            while (_transactionStack.Count > 0)
            {
                var tx = _transactionStack.Pop();
                await tx.DisposeAsync();
            }
        }

        // Inner helper implementation for dynamic generic repository resolution
        private class GenericRepositoryImplementation<T> : RepositoryEFCoreBase<T> where T : class
        {
            public GenericRepositoryImplementation(UniCoreDbContext dbContext, IMapper mapper)
                : base(dbContext, mapper)
            {
            }
        }
    }
}
