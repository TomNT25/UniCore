using System.Net.Http.Json;
using UniCore.Application.Contract.External;
using UniCore.Application.DTO.External;

namespace UniCore.Infrastructure.External
{
    public class GoogleAuthProviderClient : IGoogleAuthProviderClient
    {
        private readonly HttpClient _httpClient;

        public GoogleAuthProviderClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GoogleUserInfoDTO?> VerifyIdTokenAsync(string idToken, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestBody = new { IdToken = idToken };
                var response = await _httpClient.PostAsJsonAsync("google-provider/verify-token", requestBody, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadFromJsonAsync<GoogleUserInfoDTO>(cancellationToken: cancellationToken);
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
