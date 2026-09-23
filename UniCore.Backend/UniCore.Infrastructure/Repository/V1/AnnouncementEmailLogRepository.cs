using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class AnnouncementEmailLogRepository : RepositoryEFCoreBase<AnnouncementEmailLog>, IAnnouncementEmailLogRepository
    {
        public AnnouncementEmailLogRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task AddRangeLogsAsync(IEnumerable<AnnouncementEmailLog> logs, CancellationToken cancellationToken = default)
        {
            var list = logs.ToList();
            if (list.Count == 0)
            {
                return;
            }

            await _dbSet.AddRangeAsync(list, cancellationToken);
            await _UniCoreDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<AnnouncementEmailLog>> GetByAnnouncementIdAsync(
            string announcementId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(l => l.AnnouncementId == announcementId)
                .OrderByDescending(l => l.SentAt ?? l.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
