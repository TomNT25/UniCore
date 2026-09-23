using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class UserRepository : RepositoryEFCoreBase<User>, IUserRepository
    {
        public UserRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(e => e.UserRoles)
                .ThenInclude(e => e.Role)
                .Include(e => e.UserPermissions)
                .ThenInclude(e => e.Permission)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(e => e.UserRoles)
                .ThenInclude(e => e.Role)
                .Include(e => e.UserPermissions)
                .ThenInclude(e => e.Permission)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(e => e.UserRoles)
                .ThenInclude(e => e.Role)
                .Include(e => e.UserPermissions)
                .ThenInclude(e => e.Permission)
                .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        }
    }
}
