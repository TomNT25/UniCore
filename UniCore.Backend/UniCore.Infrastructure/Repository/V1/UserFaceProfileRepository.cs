using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class UserFaceProfileRepository : RepositoryEFCoreBase<UserFaceProfile>, IUserFaceProfileRepository
    {
        public UserFaceProfileRepository(UniCoreDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<UserFaceProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        }

        public async Task<UserFaceProfile> UpsertEnrollmentAsync(
            string userId,
            string embeddingId,
            string modelVersion,
            string? createdBy = null,
            CancellationToken cancellationToken = default)
        {
            var existing = await GetByUserIdAsync(userId, cancellationToken);

            if (existing == null)
            {
                // Create new profile
                var profile = new UserFaceProfile
                {
                    UserId = userId,
                    Status = FaceAuthConstants.Status.PendingPin,
                    EmbeddingId = embeddingId,
                    ModelVersion = modelVersion,
                    EnrolledAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                };

                await AddAsync(profile, cancellationToken);
                await SaveChangesAsync(cancellationToken);
                return profile;
            }
            else
            {
                // Update existing profile (re-enrollment)
                existing.EmbeddingId = embeddingId;
                existing.ModelVersion = modelVersion;
                existing.Status = FaceAuthConstants.Status.PendingPin;
                existing.EnrolledAt = DateTime.UtcNow;
                existing.UpdatedAt = DateTime.UtcNow;
                existing.UpdatedBy = createdBy;
                // Reset PIN on re-enrollment
                existing.PinHash = null;
                existing.PinSetAt = null;
                existing.FailedPinAttempts = 0;
                existing.PinLockoutEnd = null;

                await UpdateAsync(existing, cancellationToken);
                await SaveChangesAsync(cancellationToken);
                return existing;
            }
        }

        public async Task<UserFaceProfile> SetPinAsync(
            string userId,
            string pinHash,
            string? updatedBy = null,
            CancellationToken cancellationToken = default)
        {
            var profile = await GetByUserIdAsync(userId, cancellationToken)
                ?? throw new InvalidOperationException($"Face profile not found for user {userId}");

            profile.PinHash = pinHash;
            profile.PinSetAt = DateTime.UtcNow;
            profile.Status = FaceAuthConstants.Status.Enrolled;
            profile.FailedPinAttempts = 0;
            profile.PinLockoutEnd = null;
            profile.UpdatedAt = DateTime.UtcNow;
            profile.UpdatedBy = updatedBy;

            await UpdateAsync(profile, cancellationToken);
            await SaveChangesAsync(cancellationToken);
            return profile;
        }

        public async Task<UserFaceProfile> IncrementFailedPinAttemptsAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            var profile = await GetByUserIdAsync(userId, cancellationToken)
                ?? throw new InvalidOperationException($"Face profile not found for user {userId}");

            profile.FailedPinAttempts++;
            profile.UpdatedAt = DateTime.UtcNow;

            // Lock if exceeded max attempts
            if (profile.FailedPinAttempts >= FaceAuthConstants.Pin.MaxFailedAttempts)
            {
                profile.PinLockoutEnd = DateTime.UtcNow.AddMinutes(FaceAuthConstants.Pin.LockoutMinutes);
            }

            await UpdateAsync(profile, cancellationToken);
            await SaveChangesAsync(cancellationToken);
            return profile;
        }

        public async Task<UserFaceProfile> ResetFailedPinAttemptsAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            var profile = await GetByUserIdAsync(userId, cancellationToken)
                ?? throw new InvalidOperationException($"Face profile not found for user {userId}");

            profile.FailedPinAttempts = 0;
            profile.PinLockoutEnd = null;
            profile.UpdatedAt = DateTime.UtcNow;

            await UpdateAsync(profile, cancellationToken);
            await SaveChangesAsync(cancellationToken);
            return profile;
        }

        public async Task<UserFaceProfile> SuspendAsync(
            string userId,
            string? updatedBy = null,
            CancellationToken cancellationToken = default)
        {
            var profile = await GetByUserIdAsync(userId, cancellationToken)
                ?? throw new InvalidOperationException($"Face profile not found for user {userId}");

            profile.Status = FaceAuthConstants.Status.Suspended;
            profile.UpdatedAt = DateTime.UtcNow;
            profile.UpdatedBy = updatedBy;

            await UpdateAsync(profile, cancellationToken);
            await SaveChangesAsync(cancellationToken);
            return profile;
        }
    }
}
