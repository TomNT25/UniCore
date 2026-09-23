using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Service.v1
{
    public interface IAnnouncementDeliveryService
    {
        /// <summary>
        /// Creates read-tracking rows and/or sends whitelist-filtered emails for IMPORTANT/URGENT.
        /// SPECIFIC_STUDENTS always persisted; other NORMAL scopes skip tracking.
        /// </summary>
        Task ApplyAudienceSideEffectsAsync(
            Announcement announcement,
            IEnumerable<string>? targetStudentIds,
            CancellationToken cancellationToken = default);
    }
}
