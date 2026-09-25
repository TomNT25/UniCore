using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IUserMfaSettingRepository : IRepository<UserMfaSetting>
    {
        
           Task<IEnumerable<UserMfaSetting?>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}
