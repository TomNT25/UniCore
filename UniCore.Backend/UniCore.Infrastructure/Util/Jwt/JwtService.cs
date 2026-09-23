using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UniCore.Application.Contract.Util;
using UniCore.Application.DTO.Entity;
using UniCore.Infrastructure.Util.Jwt;

namespace UniCore.Infrastructure.Util
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _options;
        private readonly SymmetricSecurityKey _securityKey;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JwtService(IConfiguration configuration)
        {
            _options = new JwtOptions();
            var section = configuration.GetSection("Jwt");
            section.Bind(_options);

            if (string.IsNullOrWhiteSpace(_options.Secret) || _options.Secret.Length < 32)
                throw new InvalidOperationException("JWT Secret must be configured and at least 256 bits (32 characters) long.");

            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateAccessToken(UserDTO user)
        {
            var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimType.UserName, user.Username),
                new Claim(ClaimType.Subject, user.Code),
                new Claim(ClaimType.CustomerId, ClaimType.CustomerId),
                new Claim(ClaimType.Office365TenantId, ClaimType.Office365TenantId),
                new Claim(ClaimType.Organization, ClaimType.Organization),
                new Claim(ClaimType.UserObjectId, user.Id),
                new Claim(ClaimType.Language, ClaimType.Language),
                new Claim(ClaimType.DisplayName, ClaimType.DisplayName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            if (user.Roles != null && user.Roles.Any())
            {
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimType.Role, role.Name));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_options.TokenExpireMinutes),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = credentials
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            try
            {
                var tokenValidationParameters = GetValidationParameters(validateLifetime: false);

                var principal = _tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public bool ValidateToken(string token)
        {
            try
            {
                _tokenHandler.ValidateToken(token, GetValidationParameters(validateLifetime: true), out _);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private TokenValidationParameters GetValidationParameters(bool validateLifetime)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _securityKey,
                ValidateIssuer = true,
                ValidIssuer = _options.Issuer,
                ValidateAudience = true,
                ValidAudience = _options.Audience,
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}