using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class UserPersonIdRepository : RepositoryEFCoreBase<UserPersonId>, IUserPersonIdRepository
    {
        public UserPersonIdRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<UserPersonId?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
        }

        public async Task<UserPersonId?> GetByIdNumberAsync(string idNumber, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdNumber == idNumber, cancellationToken);
        }

        public async Task<UserPersonId> UpsertFromOcrAsync(UserPersonId entity, CancellationToken cancellationToken = default)
        {
            var existing = await _dbSet.FirstOrDefaultAsync(p => p.UserId == entity.UserId, cancellationToken);
            if (existing == null)
            {
                await _dbSet.AddAsync(entity, cancellationToken);
                await _UniCoreDbContext.SaveChangesAsync(cancellationToken);
                return entity;
            }

            existing.IdNumber = entity.IdNumber;
            existing.FullName = entity.FullName;
            existing.CardType = entity.CardType;
            existing.BirthDate = entity.BirthDate;
            existing.Gender = entity.Gender;
            existing.Nationality = entity.Nationality;
            existing.PlaceOfOrigin = entity.PlaceOfOrigin;
            existing.PlaceOfResidence = entity.PlaceOfResidence;
            existing.ExpireDate = entity.ExpireDate;
            existing.VerificationStatus = entity.VerificationStatus;
            existing.VerifiedAt = entity.VerifiedAt;
            existing.VerifiedBy = entity.VerifiedBy;
            existing.IsActive = entity.IsActive;
            existing.UpdatedAt = entity.UpdatedAt;
            existing.UpdatedBy = entity.UpdatedBy;

            await _UniCoreDbContext.SaveChangesAsync(cancellationToken);
            return existing;
        }
    }
}
