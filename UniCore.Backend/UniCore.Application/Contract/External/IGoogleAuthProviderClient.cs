using UniCore.Application.DTO.External;

namespace UniCore.Application.Contract.External
{
    public interface IGoogleAuthProviderClient
    {
        Task<GoogleUserInfoDTO?> VerifyIdTokenAsync(string idToken, CancellationToken cancellationToken = default);
    }
}
