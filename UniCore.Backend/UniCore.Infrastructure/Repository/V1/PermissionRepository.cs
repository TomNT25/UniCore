using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class PermissionRepository : RepositoryEFCoreBase<Permission>, IPermissionRepository
    {
        public PermissionRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<Permission?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Permission?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
        }

        public async Task<List<Permission>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(p => ids.Contains(p.Id)).ToListAsync(cancellationToken);
        }

        public async Task<List<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }
    }
}
