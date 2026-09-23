using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IUserFaceProfileRepository : IRepository<UserFaceProfile>
    {
        /// <summary>
        /// Get face profile by user ID.
        /// </summary>
        Task<UserFaceProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create or update face profile after successful AI enrollment.
        /// Sets status to FACE_PENDING_PIN if new enrollment.
        /// </summary>
        Task<UserFaceProfile> UpsertEnrollmentAsync(
            string userId,
            string embeddingId,
            string modelVersion,
            string? createdBy = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Set PIN hash and update status to FACE_ENROLLED.
        /// </summary>
        Task<UserFaceProfile> SetPinAsync(
            string userId,
            string pinHash,
            string? updatedBy = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Increment failed PIN attempts and lock if threshold exceeded.
        /// </summary>
        Task<UserFaceProfile> IncrementFailedPinAttemptsAsync(
            string userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Reset failed PIN attempts on successful verification.
        /// </summary>
        Task<UserFaceProfile> ResetFailedPinAttemptsAsync(
            string userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Suspend face profile (e.g., due to security concerns).
        /// </summary>
        Task<UserFaceProfile> SuspendAsync(
            string userId,
            string? updatedBy = null,
            CancellationToken cancellationToken = default);
    }
}
