using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    /// <summary>
    /// Repository for face authentication audit logs.
    /// </summary>
    public interface IFaceAuthLogRepository : IRepository<FaceAuthLog>
    {
        /// <summary>
        /// Add an audit log entry.
        /// </summary>
        Task AddLogAsync(FaceAuthLog log, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get login attempt count by IP in the specified time window.
        /// </summary>
        Task<int> GetLoginAttemptCountByIpAsync(
            string ipAddress,
            TimeSpan window,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get login attempt count by user in the specified time window.
        /// </summary>
        Task<int> GetLoginAttemptCountByUserAsync(
            string userId,
            TimeSpan window,
            CancellationToken cancellationToken = default);
    }
}
