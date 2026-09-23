using System.Security.Claims;
using UniCore.Helper.Constant;

namespace UniCore.Infrastructure.Util.Jwt
{
    public class JwtOptions
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TokenExpireMinutes { get; set; }
        public int RefreshTokenExpireDays { get; set; }
    }

    public static class ClaimType
    {
        #region claim types
        public static readonly string Realm = AuthConstants.Claims.Realm;
        public static readonly string Scope = AuthConstants.Claims.Scope;
        public static readonly string Issuer = "iss";
        public static readonly string Audience = "aud";
        public static readonly string CustomerId = AuthConstants.Claims.CustomerId;
        public static readonly string ClientId = AuthConstants.Claims.ClientId;
        public static readonly string UserName = ClaimTypes.Upn;
        public static readonly string Language = AuthConstants.Claims.Language;
        public static readonly string DisplayName = ClaimTypes.Name;
        public static readonly string Email = ClaimTypes.Email;
        public static readonly string Organization = AuthConstants.Claims.Organization;
        public static readonly string UserObjectId = AuthConstants.Claims.UserId;
        public static readonly string Office365TenantId = AuthConstants.Claims.Office365TenantId;
        public static readonly string UserGroups = AuthConstants.Claims.UserGroups;
        public static readonly string CloudUserId = AuthConstants.Claims.CloudUserId;
        public static readonly string Role = ClaimTypes.Role;
        #endregion
    }
}
