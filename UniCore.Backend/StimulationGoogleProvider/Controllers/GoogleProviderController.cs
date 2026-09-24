using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace StimulationGoogleProvider.Controllers
{
    /// <summary>
    /// Simulated Google OAuth Identity Provider endpoints.
    /// Used for testing Google authentication flows locally without connecting to live Google servers.
    /// </summary>
    [ApiController]
    [Route("google-provider")]
    [Produces("application/json")]
    public class GoogleProviderController : ControllerBase
    {
        /// <summary>
        /// Request payload for generating a simulated Google ID Token.
        /// </summary>
        public class SimulateTokenRequest
        {
            /// <summary>
            /// User's email address (e.g. john.doe@example.com).
            /// </summary>
            public string Email { get; set; } = string.Empty;

            /// <summary>
            /// User's display name.
            /// </summary>
            public string Name { get; set; } = string.Empty;

            /// <summary>
            /// Optional URL to the user's avatar image.
            /// </summary>
            public string? Picture { get; set; }

            /// <summary>
            /// Optional Google unique subject identifier (sub claim). If not provided, a deterministic sub is generated.
            /// </summary>
            public string? Sub { get; set; }
        }

        /// <summary>
        /// Response payload containing the simulated Google ID Token.
        /// </summary>
        public class SimulateTokenResponse
        {
            /// <summary>
            /// Encoded simulated JWT ID token.
            /// </summary>
            public string IdToken { get; set; } = string.Empty;

            /// <summary>
            /// Google subject identifier.
            /// </summary>
            public string Sub { get; set; } = string.Empty;

            /// <summary>
            /// User email.
            /// </summary>
            public string Email { get; set; } = string.Empty;

            /// <summary>
            /// User name.
            /// </summary>
            public string Name { get; set; } = string.Empty;

            /// <summary>
            /// Avatar URL.
            /// </summary>
            public string? Picture { get; set; }
        }

        /// <summary>
        /// Request payload for verifying a simulated Google ID Token.
        /// </summary>
        public class VerifyTokenRequest
        {
            /// <summary>
            /// The JWT ID token to verify.
            /// </summary>
            public string IdToken { get; set; } = string.Empty;
        }

        /// <summary>
        /// Response payload resulting from ID token verification.
        /// </summary>
        public class VerifyTokenResponse
        {
            /// <summary>
            /// Indicates whether the token was successfully verified.
            /// </summary>
            public bool IsValid { get; set; }

            /// <summary>
            /// Google subject identifier (sub).
            /// </summary>
            public string? Sub { get; set; }

            /// <summary>
            /// User email address.
            /// </summary>
            public string? Email { get; set; }

            /// <summary>
            /// Whether the user's email was verified by Google.
            /// </summary>
            public bool EmailVerified { get; set; }

            /// <summary>
            /// User display name.
            /// </summary>
            public string? Name { get; set; }

            /// <summary>
            /// User avatar picture URL.
            /// </summary>
            public string? Picture { get; set; }

            /// <summary>
            /// Token issuer (iss claim).
            /// </summary>
            public string? Issuer { get; set; }
        }

        /// <summary>
        /// Issue a simulated Google ID Token
        /// </summary>
        /// <param name="request">User details to encode into the mock Google ID token.</param>
        /// <response code="200">Token generated successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        [HttpPost("simulate-token")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(SimulateTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <param name="request">The ID token string to decode and verify.</param>
        /// <response code="200">Token verified successfully.</response>
        /// <response code="400">Token is invalid or malformed.</response>
        [HttpPost("verify-token")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(VerifyTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(VerifyTokenResponse), StatusCodes.Status400BadRequest)]
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
