using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class UserRoleRepository : RepositoryEFCoreBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<List<UserRole>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserRole>> GetByUserAndRoleIdsAsync(string userId, IEnumerable<string> roleIds, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(ur => ur.UserId == userId && roleIds.Contains(ur.RoleId))
                .ToListAsync(cancellationToken);
        }
    }
}
