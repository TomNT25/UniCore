using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class UserTokenRepository : RepositoryEFCoreBase<UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<UserToken?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken && t.IsActive, cancellationToken);
        }

        public async Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var token = await GetByRefreshTokenAsync(refreshToken, cancellationToken);
            if (token != null)
            {
                token.RevokedAt = DateTime.UtcNow;
                token.IsActive = false;
                _dbSet.Update(token);
                await _UniCoreDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
