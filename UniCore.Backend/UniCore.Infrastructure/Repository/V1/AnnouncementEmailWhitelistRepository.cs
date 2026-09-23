using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class AnnouncementEmailWhitelistRepository : RepositoryEFCoreBase<AnnouncementEmailWhitelist>, IAnnouncementEmailWhitelistRepository
    {
        public AnnouncementEmailWhitelistRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<List<AnnouncementEmailWhitelist>> GetActiveEntriesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(w => w.Status == AnnouncementConstants.Whitelist.Active)
                .ToListAsync(cancellationToken);
        }
    }
}
