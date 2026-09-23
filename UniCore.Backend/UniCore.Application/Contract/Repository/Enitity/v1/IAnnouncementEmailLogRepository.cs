using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IAnnouncementEmailLogRepository : IRepository<AnnouncementEmailLog>
    {
        Task AddRangeLogsAsync(IEnumerable<AnnouncementEmailLog> logs, CancellationToken cancellationToken = default);

        Task<List<AnnouncementEmailLog>> GetByAnnouncementIdAsync(
            string announcementId,
            CancellationToken cancellationToken = default);
    }
}
