using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class SchoolClassRepository : RepositoryEFCoreBase<SchoolClass>, ISchoolClassRepository
    {
        public SchoolClassRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<SchoolClass?> GetInfoByIdAsync(
            string classIds,
            CancellationToken ct = default
            )
        {
            var results = await _dbSet
                .Where(s => s.Id.Equals(classIds))
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);

            return results;
        }
    }
}
