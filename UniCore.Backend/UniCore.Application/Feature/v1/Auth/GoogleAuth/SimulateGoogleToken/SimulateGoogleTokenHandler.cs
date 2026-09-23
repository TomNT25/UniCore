using FluentValidation;
using System.Text;
using System.Text.Json;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Auth.GoogleAuth.SimulateGoogleToken
{
    public class SimulateGoogleTokenHandler : IRequestHandler<SimulateGoogleTokenRequestDTO, SimulateGoogleTokenResponseDTO>
    {
        private readonly IValidator<SimulateGoogleTokenRequestDTO> _validator;

        public SimulateGoogleTokenHandler(IValidator<SimulateGoogleTokenRequestDTO> validator)
        {
            _validator = validator;
        }

        public async Task<SimulateGoogleTokenResponseDTO> HandleAsync(SimulateGoogleTokenRequestDTO request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var sub = string.IsNullOrWhiteSpace(request.Sub)
                ? $"google-{Math.Abs(request.Email.GetHashCode())}"
                : request.Sub;

            var header = new { alg = "RS256", typ = "JWT", kid = "simulated-google-key-id" };
            var payload = new
            {
                iss = "https://accounts.google.com",
                azp = "unicore-client-id.apps.googleusercontent.com",
                aud = "unicore-client-id.apps.googleusercontent.com",
                sub = sub,
                email = request.Email,
                email_verified = true,
                name = request.Name,
                picture = request.Picture ?? "https://lh3.googleusercontent.com/a/default-avatar",
                iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                exp = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds()
            };

            var headerJson = JsonSerializer.Serialize(header);
            var payloadJson = JsonSerializer.Serialize(payload);

            var headerB64 = Base64UrlEncode(Encoding.UTF8.GetBytes(headerJson));
            var payloadB64 = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));
            var mockSignature = Base64UrlEncode(Encoding.UTF8.GetBytes("SIMULATED_GOOGLE_SIGNATURE"));

            var idToken = $"{headerB64}.{payloadB64}.{mockSignature}";

            return new SimulateGoogleTokenResponseDTO
            {
                IdToken = idToken,
                Sub = sub,
                Email = request.Email,
                Name = request.Name,
                Picture = payload.picture
            };
        }

        private static string Base64UrlEncode(byte[] arg)
        {
            string s = Convert.ToBase64String(arg);
            s = s.Split('=')[0]; // Remove any trailing '='s
            s = s.Replace('+', '-'); // 62nd char of encoding
            s = s.Replace('/', '_'); // 63rd char of encoding
            return s;
        }
    }
}
