using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IUserPersonIdRepository : IRepository<UserPersonId>
    {
        Task<UserPersonId?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<UserPersonId?> GetByIdNumberAsync(string idNumber, CancellationToken cancellationToken = default);
        Task<UserPersonId> UpsertFromOcrAsync(UserPersonId entity, CancellationToken cancellationToken = default);
    }
}
