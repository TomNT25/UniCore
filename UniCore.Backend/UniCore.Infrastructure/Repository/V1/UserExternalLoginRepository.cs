using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class UserExternalLoginRepository : RepositoryEFCoreBase<UserExternalLogin>, IUserExternalLoginRepository
    {
        public UserExternalLoginRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<UserExternalLogin?> GetByProviderAndProviderUserIdAsync(string provider, string providerUserId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Provider == provider && x.ProviderUserId == providerUserId, cancellationToken);
        }
    }
}
