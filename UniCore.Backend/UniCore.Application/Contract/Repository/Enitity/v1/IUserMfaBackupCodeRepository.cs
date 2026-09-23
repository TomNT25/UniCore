using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IUserMfaBackupCodeRepository : IRepository<UserMfaBackupCode>
    {
        
          Task<List<UserMfaBackupCode>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}
