using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class AnnouncementRepository : RepositoryEFCoreBase<Announcement>, IAnnouncementRepository
    {
        public AnnouncementRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<Announcement?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<Announcement?> GetByIdWithDetailsAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(a => a.AnnouncementStudents)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<List<Announcement>> GetPublishedPublicAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _dbSet
                .AsNoTracking()
                .Where(a =>
                    a.ScopeType == AnnouncementConstants.Scope.Public &&
                    a.PublishDate <= now &&
                    a.ExpiredDate > now)
                .OrderByDescending(a => a.PublishDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<Announcement?> GetPublishedPublicByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(a =>
                    a.Id == id &&
                    a.ScopeType == AnnouncementConstants.Scope.Public &&
                    a.PublishDate <= now &&
                    a.ExpiredDate > now,
                    cancellationToken);
        }

        public async Task<(List<Announcement> Items, int TotalCount)> GetAnnouncementsForStudentAsync(
            string studentId,
            string? typeFilter,
            string? timeStatus,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var query = _dbSet
                .AsNoTracking()
                .Where(a =>
                    a.ScopeType == AnnouncementConstants.Scope.Public
                    || a.ScopeType == AnnouncementConstants.Scope.Students
                    || a.AnnouncementStudents.Any(s => s.StudentId == studentId));

            var normalizedTimeStatus = string.IsNullOrWhiteSpace(timeStatus)
                ? "active"
                : timeStatus.Trim().ToLowerInvariant();

            query = normalizedTimeStatus switch
            {
                "expired" => query.Where(a => now >= a.ExpiredDate),
                "active" => query.Where(a => a.PublishDate <= now && a.ExpiredDate > now),
                _ => query.Where(a => a.PublishDate <= now && a.ExpiredDate > now)
            };

            if (!string.IsNullOrWhiteSpace(typeFilter))
            {
                query = query.Where(a => a.Type == typeFilter);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(a => a.Type == AnnouncementConstants.Type.Urgent ? 3 :
                                        a.Type == AnnouncementConstants.Type.Important ? 2 : 1)
                .ThenByDescending(a => a.PublishDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<Announcement?> GetAnnouncementVisibleToStudentAsync(
            string studentId,
            string announcementId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(a =>
                    a.Id == announcementId
                    && (a.ScopeType == AnnouncementConstants.Scope.Public
                        || a.ScopeType == AnnouncementConstants.Scope.Students
                        || a.AnnouncementStudents.Any(s => s.StudentId == studentId)),
                    cancellationToken);
        }
    }
}
