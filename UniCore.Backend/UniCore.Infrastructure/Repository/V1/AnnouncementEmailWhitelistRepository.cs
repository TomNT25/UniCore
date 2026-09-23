using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class AnnouncementEmailWhitelistRepository : RepositoryEFCoreBase<AnnouncementEmailWhitelist>, IAnnouncementEmailWhitelistRepository
    {
        public AnnouncementEmailWhitelistRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
