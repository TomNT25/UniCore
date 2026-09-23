namespace UniCore.Helper.Constant
{
    public static class AuthConstants
    {
        public static class JwtConfig
        {
            public const string SecretKeyPath = "Jwt:Secret";
            public const string IssuerPath = "Jwt:Issuer";
            public const string AudiencePath = "Jwt:Audience";
            public const string TokenExpireMinutesPath = "Jwt:TokenExpireMinutes";
            public const string RefreshTokenExpireDaysPath = "Jwt:RefreshTokenExpireDays";

            public const string DefaultIssuer = "UniCore";
            public const string DefaultAudience = "UniCore-Users";
            public const int DefaultTokenExpireMinutes = 60;
            public const int DefaultRefreshTokenExpireDays = 7;
        }

        public static class Claims
        {
            public const string UserId = "uid";
            public const string Realm = "realm";
            public const string Scope = "scope";
            public const string CustomerId = "customer_id";
            public const string ClientId = "client_id";
            public const string Language = "language";
            public const string Organization = "organization";
            public const string Office365TenantId = "office365TenantId";
            public const string UserGroups = "user_groups";
            public const string CloudUserId = "objectid";
        }

        public static class Headers
        {
            public const string TokenExpired = "Token-Expired";
            public const string ValueTrue = "true";
        }

        public static class Providers
        {
            public const string System = "system";
        }
    }
}
