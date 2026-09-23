using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1;

public class UserPermissionRepository : RepositoryEFCoreBase<UserPermission>, IUserPermissionRepository
{
    public UserPermissionRepository(
        UniCoreDbContext UniCoreDbContext,
        IMapper mapper) : base(UniCoreDbContext, mapper)
    {
    }

    public async Task<IEnumerable<UserPermission>> GetByUserIDAsync(string userId, CancellationToken cancellationToken = default)
    {
        var result = await _dbSet.Where(x => x.UserId.Equals(userId))
                                .AsNoTracking()
                                .Include(x => x.Permission)
                                .ToListAsync(cancellationToken);
        return result;
    }

    public async Task<List<UserPermission>> GetByUserAndPermissionIdsAsync(string userId, IEnumerable<string> permissionIds, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(up => up.UserId == userId && permissionIds.Contains(up.PermissionId))
            .ToListAsync(cancellationToken);
    }
}