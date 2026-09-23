using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace StimulationGoogleProvider.Controllers
{
    [ApiController]
    [Route("google-provider")]
    public class GoogleProviderController : ControllerBase
    {
        public class SimulateTokenRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string? Picture { get; set; }
            public string? Sub { get; set; }
        }

        public class SimulateTokenResponse
        {
            public string IdToken { get; set; } = string.Empty;
            public string Sub { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string? Picture { get; set; }
        }

        public class VerifyTokenRequest
        {
            public string IdToken { get; set; } = string.Empty;
        }

        public class VerifyTokenResponse
        {
            public bool IsValid { get; set; }
            public string? Sub { get; set; }
            public string? Email { get; set; }
            public bool EmailVerified { get; set; }
            public string? Name { get; set; }
            public string? Picture { get; set; }
            public string? Issuer { get; set; }
        }

        /// <summary>
        /// Issue a simulated Google ID Token
        /// </summary>
        [HttpPost("simulate-token")]
        public IActionResult SimulateToken([FromBody] SimulateTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new { error = "Email and Name are required." });
            }

            var sub = string.IsNullOrWhiteSpace(request.Sub)
                ? $"google-{Math.Abs(request.Email.GetHashCode())}"
                : request.Sub;

            var header = new { alg = "RS256", typ = "JWT", kid = "google-simulated-key-1" };
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

            return Ok(new SimulateTokenResponse
            {
                IdToken = idToken,
                Sub = sub,
                Email = request.Email,
                Name = request.Name,
                Picture = payload.picture
            });
        }

        /// <summary>
        /// Verify a Google ID Token (Simulated Endpoint)
        /// </summary>
        [HttpPost("verify-token")]
        public IActionResult VerifyToken([FromBody] VerifyTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.IdToken))
            {
                return BadRequest(new { error = "IdToken is required." });
            }

            try
            {
                var parts = request.IdToken.Split('.');
                if (parts.Length < 2)
                {
                    return BadRequest(new VerifyTokenResponse { IsValid = false });
                }

                var payloadB64 = parts[1].Replace('-', '+').Replace('_', '/');
                switch (payloadB64.Length % 4)
                {
                    case 2: payloadB64 += "=="; break;
                    case 3: payloadB64 += "="; break;
                }

                var jsonBytes = Convert.FromBase64String(payloadB64);
                var jsonString = Encoding.UTF8.GetString(jsonBytes);

                using var doc = JsonDocument.Parse(jsonString);
                var root = doc.RootElement;

                var response = new VerifyTokenResponse
                {
                    IsValid = true,
                    Sub = root.TryGetProperty("sub", out var subProp) ? subProp.GetString() : null,
                    Email = root.TryGetProperty("email", out var emailProp) ? emailProp.GetString() : null,
                    EmailVerified = root.TryGetProperty("email_verified", out var evProp) && evProp.GetBoolean(),
                    Name = root.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : null,
                    Picture = root.TryGetProperty("picture", out var picProp) ? picProp.GetString() : null,
                    Issuer = root.TryGetProperty("iss", out var issProp) ? issProp.GetString() : "https://accounts.google.com"
                };

                return Ok(response);
            }
            catch
            {
                return BadRequest(new VerifyTokenResponse { IsValid = false });
            }
        }

        private static string Base64UrlEncode(byte[] arg)
        {
            string s = Convert.ToBase64String(arg);
            s = s.Split('=')[0];
            s = s.Replace('+', '-');
            s = s.Replace('/', '_');
            return s;
        }
    }
}
