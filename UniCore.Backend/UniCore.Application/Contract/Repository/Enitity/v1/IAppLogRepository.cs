using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IAppLogRepository : IRepository<AppLog>
    {
        Task<AppLog?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}
