using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class AnnouncementStudentRepository : RepositoryEFCoreBase<AnnouncementStudent>, IAnnouncementStudentRepository
    {
        public AnnouncementStudentRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<List<AnnouncementStudent>> GetByAnnouncementIdAsync(string announcementId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(s => s.Student)
                .ThenInclude(u => u.UserProfile)
                .Where(s => s.AnnouncementId == announcementId)
                .OrderBy(s => s.Student.Username)
                .ToListAsync(cancellationToken);
        }

        public async Task ReplaceRecipientsAsync(string announcementId, IEnumerable<string> studentIds, CancellationToken cancellationToken = default)
        {
            await _dbSet.Where(s => s.AnnouncementId == announcementId).ExecuteDeleteAsync(cancellationToken);

            var distinctIds = studentIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (distinctIds.Count == 0)
            {
                return;
            }

            var now = DateTime.UtcNow;
            var entities = distinctIds.Select(studentId => new AnnouncementStudent
            {
                Id = Guid.NewGuid().ToString(),
                AnnouncementId = announcementId,
                StudentId = studentId,
                IsSent = false,
                CreatedAt = now
            });

            await _dbSet.AddRangeAsync(entities, cancellationToken);
            await _UniCoreDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> CountByAnnouncementIdAsync(string announcementId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().CountAsync(s => s.AnnouncementId == announcementId, cancellationToken);
        }

        public async Task<(int Total, int Viewed, int Acknowledged)> GetDeliveryStatsAsync(string announcementId, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsNoTracking().Where(s => s.AnnouncementId == announcementId);
            var total = await query.CountAsync(cancellationToken);
            var viewed = await query.CountAsync(s => s.ViewedAt != null, cancellationToken);
            var acknowledged = await query.CountAsync(s => s.AcknowledgedAt != null, cancellationToken);
            return (total, viewed, acknowledged);
        }

        public async Task<Dictionary<string, AnnouncementStudent>> GetReadStatusesAsync(
            string studentId,
            IEnumerable<string> announcementIds,
            CancellationToken cancellationToken = default)
        {
            var ids = announcementIds.ToList();
            if (ids.Count == 0)
            {
                return new Dictionary<string, AnnouncementStudent>();
            }

            var statuses = await _dbSet
                .AsNoTracking()
                .Where(s => s.StudentId == studentId && ids.Contains(s.AnnouncementId))
                .ToListAsync(cancellationToken);

            return statuses.ToDictionary(s => s.AnnouncementId, s => s);
        }

        public async Task<DateTime> MarkAsViewedAsync(string announcementId, string studentId, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            var existing = await _dbSet.FirstOrDefaultAsync(
                s => s.AnnouncementId == announcementId && s.StudentId == studentId,
                cancellationToken);

            if (existing == null)
            {
                // Create new record if doesn't exist (for PUBLIC announcements)
                existing = new AnnouncementStudent
                {
                    Id = Guid.NewGuid().ToString(),
                    AnnouncementId = announcementId,
                    StudentId = studentId,
                    ViewedAt = now,
                    IsSent = false,
                    CreatedAt = now
                };
                await _dbSet.AddAsync(existing, cancellationToken);
            }
            else if (existing.ViewedAt == null)
            {
                existing.ViewedAt = now;
                _dbSet.Update(existing);
            }
            else
            {
                // Already viewed, return existing timestamp
                return existing.ViewedAt.Value;
            }

            await _UniCoreDbContext.SaveChangesAsync(cancellationToken);
            return now;
        }

        public async Task<DateTime> MarkAsAcknowledgedAsync(string announcementId, string studentId, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            var existing = await _dbSet.FirstOrDefaultAsync(
                s => s.AnnouncementId == announcementId && s.StudentId == studentId,
                cancellationToken);

            if (existing == null)
            {
                // Create new record if doesn't exist (for PUBLIC announcements)
                existing = new AnnouncementStudent
                {
                    Id = Guid.NewGuid().ToString(),
                    AnnouncementId = announcementId,
                    StudentId = studentId,
                    ViewedAt = now, // Also mark as viewed
                    AcknowledgedAt = now,
                    CreatedAt = now
                };
                await _dbSet.AddAsync(existing, cancellationToken);
            }
            else
            {
                // Update existing record
                if (existing.ViewedAt == null)
                {
                    existing.ViewedAt = now;
                }
                if (existing.AcknowledgedAt == null)
                {
                    existing.AcknowledgedAt = now;
                }
                else
                {
                    // Already acknowledged, return existing timestamp
                    return existing.AcknowledgedAt.Value;
                }
                _dbSet.Update(existing);
            }

            await _UniCoreDbContext.SaveChangesAsync(cancellationToken);
            return now;
        }

        public async Task<List<AnnouncementStudent>> GetUnsentWithAnnouncementAsync(
            string studentId,
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _dbSet
                .AsNoTracking()
                .Include(s => s.Announcement)
                .Where(s =>
                    s.StudentId == studentId
                    && !s.IsSent
                    && s.Announcement.PublishDate <= now
                    && s.Announcement.ExpiredDate > now)
                .OrderByDescending(s => s.Announcement.PublishDate)
                .ToListAsync(cancellationToken);
        }

        public async Task MarkAsSentAsync(IEnumerable<string> announcementStudentIds, CancellationToken cancellationToken = default)
        {
            var ids = announcementStudentIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (ids.Count == 0)
            {
                return;
            }

            await _dbSet
                .Where(s => ids.Contains(s.Id))
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(s => s.IsSent, true),
                    cancellationToken);
        }

        public async Task<AnnouncementStudent?> GetLinkAsync(
            string studentId,
            string announcementId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    s => s.StudentId == studentId && s.AnnouncementId == announcementId,
                    cancellationToken);
        }
    }
}
