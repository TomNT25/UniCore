using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UniCore.API.Middlewares.CustomMiddlewares
{
    public class DebugContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public DebugContextMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var secretKey = _configuration["Jwt:Secret"];
            if (!string.IsNullOrEmpty(secretKey) && secretKey.Length >= 32)
            {
                var requestHeaders = context.Request.Headers;
                if (!requestHeaders.ContainsKey(HeaderNames.Authorization))
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                    var tokenHandler = new JwtSecurityTokenHandler();

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimType.UserName, "admin@UniCore.com"),
                            new Claim(ClaimType.Email, "admin@UniCore.com"),
                            new Claim(ClaimType.UserObjectId, "018f4b5a-2b3c-7d4e-8f5a-6b7c8d9e0f1a"),
                            new Claim(ClaimType.DisplayName, "System Administrator"),
                            new Claim(ClaimType.Role, "Admin"),
                            new Claim(ClaimType.CustomerId, ClaimType.CustomerId),
                            new Claim(ClaimType.Office365TenantId, ClaimType.Office365TenantId),
                            new Claim(ClaimType.Organization, ClaimType.Organization),
                            new Claim(ClaimType.Language, ClaimType.Language),
                            new Claim(ClaimType.DisplayName, ClaimType.DisplayName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                        }),
                        Issuer = _configuration["Jwt:Issuer"] ?? "UniCore",
                        Audience = _configuration["Jwt:Audience"] ?? "UniCore-Users",
                        SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
                        Expires = DateTime.UtcNow.AddHours(1)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var encodedToken = tokenHandler.WriteToken(token);
                    requestHeaders.Append(HeaderNames.Authorization, "Bearer " + encodedToken);
                }
            }

            await _next(context);
        }
    }

    public static class ClaimType
    {
        public static readonly string Realm = "realm";
        public static readonly string Scope = "scope";
        public static readonly string Issuer = "iss";
        public static readonly string Audience = "aud";
        public static readonly string CustomerId = "customer_id";
        public static readonly string ClientId = "client_id";
        public static readonly string UserName = ClaimTypes.Upn;
        public static readonly string Language = "language";
        public static readonly string DisplayName = ClaimTypes.Name;
        public static readonly string Email = ClaimTypes.Email;
        public static readonly string Organization = "organization";
        public static readonly string UserObjectId = "uid";
        public static readonly string Office365TenantId = "office365TenantId";
        public static readonly string UserGroups = "user_groups";
        public static readonly string CloudUserId = "objectid";
        public static readonly string Role = ClaimTypes.Role;
    }
}
