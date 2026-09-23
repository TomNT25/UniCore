using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        Task<UserProfile?> GetByUserIDAsync(string userId, CancellationToken ct = default);

        Task<UserProfile?> GetByUserIdAsync(string userId, CancellationToken ct = default);

        Task<IEnumerable<UserProfile>?> GetInfoByIdAsync(IEnumerable<string> studentIds, CancellationToken ct = default);

    }
}
