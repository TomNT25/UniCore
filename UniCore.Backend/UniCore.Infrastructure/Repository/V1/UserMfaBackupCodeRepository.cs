using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class UserMfaBackupCodeRepository : RepositoryEFCoreBase<UserMfaBackupCode>, IUserMfaBackupCodeRepository
    {
        public UserMfaBackupCodeRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<List<UserMfaBackupCode>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(b => b.UserId == userId).ToListAsync(cancellationToken);
        }
    }
}
