using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        Task<UserProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}
