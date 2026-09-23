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
                .Include(e => e.UserProfile)
                .Include(e => e.UserRoles)
                .ThenInclude(e => e.Role)
                .Include(e => e.UserPermissions)
                .ThenInclude(e => e.Permission)
                .FirstOrDefaultAsync(u => u.Email.Equals(email), cancellationToken);
        }

        public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(e => e.UserProfile)
                .Include(e => e.UserRoles)
                .ThenInclude(e => e.Role)
                .Include(e => e.UserPermissions)
                .ThenInclude(e => e.Permission)
                .FirstOrDefaultAsync(u => u.Id.Equals(id), cancellationToken);
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(e => e.UserProfile)
                .Include(e => e.UserRoles)
                .ThenInclude(e => e.Role)
                .Include(e => e.UserPermissions)
                .ThenInclude(e => e.Permission)
                .FirstOrDefaultAsync(u => u.Username.Equals(username), cancellationToken);
        }

        public async Task<List<string>> GetActiveStudentIdsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(u =>
                    u.IsActive &&
                    u.UserRoles.Any(ur => ur.Role != null && ur.Role.Name == "Student" && ur.Role.IsActive))
                .Select(u => u.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<string>> GetActiveVerifiedStudentIdsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(u =>
                    u.IsActive &&
                    u.UserRoles.Any(ur => ur.Role != null && ur.Role.Name == "Student" && ur.Role.IsActive) &&
                    u.UserPersonId != null &&
                    u.UserPersonId.IsActive &&
                    u.UserPersonId.VerificationStatus == "VERIFIED")
                .Select(u => u.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<string>> GetActiveStudentIdsByIdsAsync(IEnumerable<string> studentIds, CancellationToken cancellationToken = default)
        {
            var ids = studentIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (ids.Count == 0)
            {
                return new List<string>();
            }

            return await _dbSet
                .AsNoTracking()
                .Where(u =>
                    ids.Contains(u.Id) &&
                    u.IsActive &&
                    u.UserRoles.Any(ur => ur.Role != null && ur.Role.Name == "Student" && ur.Role.IsActive))
                .Select(u => u.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<(string StudentId, string Email)>> GetActiveStudentEmailsByIdsAsync(
            IEnumerable<string> studentIds,
            CancellationToken cancellationToken = default)
        {
            var ids = studentIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (ids.Count == 0)
            {
                return new List<(string, string)>();
            }

            var rows = await _dbSet
                .AsNoTracking()
                .Where(u =>
                    ids.Contains(u.Id) &&
                    u.IsActive &&
                    !string.IsNullOrWhiteSpace(u.Email) &&
                    u.UserRoles.Any(ur => ur.Role != null && ur.Role.Name == "Student" && ur.Role.IsActive))
                .Select(u => new { u.Id, u.Email })
                .ToListAsync(cancellationToken);

            return rows.Select(r => (r.Id, r.Email)).ToList();
        }

        public async Task<List<string>> GetActiveVerifiedStudentIdsByIdsAsync(
            IEnumerable<string> studentIds,
            CancellationToken cancellationToken = default)
        {
            var ids = studentIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (ids.Count == 0)
            {
                return new List<string>();
            }

            return await _dbSet
                .AsNoTracking()
                .Where(u =>
                    ids.Contains(u.Id) &&
                    u.IsActive &&
                    u.UserRoles.Any(ur => ur.Role != null && ur.Role.Name == "Student" && ur.Role.IsActive) &&
                    u.UserPersonId != null &&
                    u.UserPersonId.IsActive &&
                    u.UserPersonId.VerificationStatus == "VERIFIED")
                .Select(u => u.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<(List<User> Items, int TotalCount)> SearchActiveVerifiedStudentsAsync(
            string? search,
            int limit,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(u => u.UserProfile)
                .Include(u => u.UserPersonId)
                .Where(u =>
                    u.IsActive &&
                    u.UserRoles.Any(ur => ur.Role != null && ur.Role.Name == "Student" && ur.Role.IsActive) &&
                    u.UserPersonId != null &&
                    u.UserPersonId.IsActive &&
                    u.UserPersonId.VerificationStatus == "VERIFIED");

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(u =>
                    EF.Functions.Like(u.Username, $"%{term}%") ||
                    (u.Code != null && EF.Functions.Like(u.Code, $"%{term}%")) ||
                    (u.UserProfile != null && u.UserProfile.FullName != null &&
                     EF.Functions.Like(u.UserProfile.FullName, $"%{term}%")) ||
                    (u.UserProfile != null && u.UserProfile.FirstName != null &&
                     EF.Functions.Like(u.UserProfile.FirstName, $"%{term}%")) ||
                    (u.UserProfile != null && u.UserProfile.LastName != null &&
                     EF.Functions.Like(u.UserProfile.LastName, $"%{term}%")));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderBy(u => u.Username)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
