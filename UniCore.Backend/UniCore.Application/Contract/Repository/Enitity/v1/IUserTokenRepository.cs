using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IUserTokenRepository : IRepository<UserToken>
    {
        Task<UserToken?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
