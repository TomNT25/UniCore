using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IUserExternalLoginRepository : IRepository<UserExternalLogin>
    {
        Task<UserExternalLogin?> GetByProviderAndProviderUserIdAsync(string provider, string providerUserId, CancellationToken cancellationToken = default);
    }
}
