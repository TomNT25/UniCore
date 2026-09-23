using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class FaceAuthLogRepository : RepositoryEFCoreBase<FaceAuthLog>, IFaceAuthLogRepository
    {
        public FaceAuthLogRepository(UniCoreDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task AddLogAsync(FaceAuthLog log, CancellationToken cancellationToken = default)
        {
            await AddAsync(log, cancellationToken);
            await SaveChangesAsync(cancellationToken);
        }

        public async Task<int> GetLoginAttemptCountByIpAsync(
            string ipAddress,
            TimeSpan window,
            CancellationToken cancellationToken = default)
        {
            var cutoff = DateTime.UtcNow - window;
            return await _dbSet
                .AsNoTracking()
                .Where(l =>
                    l.IpAddress == ipAddress &&
                    l.Action == "LOGIN" &&
                    l.CreatedAt >= cutoff)
                .CountAsync(cancellationToken);
        }

        public async Task<int> GetLoginAttemptCountByUserAsync(
            string userId,
            TimeSpan window,
            CancellationToken cancellationToken = default)
        {
            var cutoff = DateTime.UtcNow - window;
            return await _dbSet
                .AsNoTracking()
                .Where(l =>
                    l.UserId == userId &&
                    l.Action == "LOGIN" &&
                    l.CreatedAt >= cutoff)
                .CountAsync(cancellationToken);
        }
    }
}
