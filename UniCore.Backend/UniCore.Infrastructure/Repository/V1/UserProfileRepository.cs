using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class UserProfileRepository : RepositoryEFCoreBase<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<UserProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(up => up.UserId == userId, cancellationToken);
        }
    }
}
