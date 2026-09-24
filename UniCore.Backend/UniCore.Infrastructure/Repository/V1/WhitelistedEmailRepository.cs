using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class WhitelistedEmailRepository : RepositoryEFCoreBase<WhitelistedEmail>, IWhitelistedEmailRepository
    {
        public WhitelistedEmailRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<List<WhitelistedEmail>> GetActiveEntriesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(w => w.IsActive == true)
                .ToListAsync(cancellationToken);
        }
    }
}
