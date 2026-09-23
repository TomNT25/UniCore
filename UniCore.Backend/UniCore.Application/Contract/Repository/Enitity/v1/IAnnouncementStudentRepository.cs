using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IAnnouncementStudentRepository : IRepository<AnnouncementStudent>
    {
        Task<List<AnnouncementStudent>> GetByAnnouncementIdAsync(string announcementId, CancellationToken cancellationToken = default);
        Task ReplaceRecipientsAsync(string announcementId, IEnumerable<string> studentIds, CancellationToken cancellationToken = default);
        Task<int> CountByAnnouncementIdAsync(string announcementId, CancellationToken cancellationToken = default);
        Task<(int Total, int Viewed, int Acknowledged)> GetDeliveryStatsAsync(string announcementId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get read statuses for multiple announcements for a student.
        /// </summary>
        Task<Dictionary<string, AnnouncementStudent>> GetReadStatusesAsync(
            string studentId,
            IEnumerable<string> announcementIds,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Mark announcement as viewed by student. Returns the viewed_at timestamp.
        /// </summary>
        Task<DateTime> MarkAsViewedAsync(string announcementId, string studentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Mark announcement as acknowledged by student. Returns the acknowledged_at timestamp.
        /// </summary>
        Task<DateTime> MarkAsAcknowledgedAsync(string announcementId, string studentId, CancellationToken cancellationToken = default);

        Task<List<AnnouncementStudent>> GetUnsentWithAnnouncementAsync(
            string studentId,
            CancellationToken cancellationToken = default);

        Task MarkAsSentAsync(IEnumerable<string> announcementStudentIds, CancellationToken cancellationToken = default);

        Task<AnnouncementStudent?> GetLinkAsync(
            string studentId,
            string announcementId,
            CancellationToken cancellationToken = default);
    }
}
