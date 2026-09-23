using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using UniCore.Application.DTO;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Extensions
{
    public static class AuthenticationPolicyExtension
    {
        public static void AddAuthenticationPolicyService(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = configuration[AuthConstants.JwtConfig.SecretKeyPath]
                ?? throw new InvalidOperationException(ExceptionConstants.MissingJwtSecretMessage);
            var issuer = configuration[AuthConstants.JwtConfig.IssuerPath] ?? AuthConstants.JwtConfig.DefaultIssuer;
            var audience = configuration[AuthConstants.JwtConfig.AudiencePath] ?? AuthConstants.JwtConfig.DefaultAudience;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.Upn
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Append(AuthConstants.Headers.TokenExpired, AuthConstants.Headers.ValueTrue);
                        }
                        return Task.CompletedTask;
                    },
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var localizer = context.HttpContext.RequestServices.GetRequiredService<IJsonStringLocalizer>();
                        var message = localizer.GetString(MessageConstants.System.UnauthorizedAccess);
                        var response = BaseAPIResponse<object>.Failure(message, StatusCodes.Status401Unauthorized);

                        var jsonOptions = new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                        };

                        await System.Text.Json.JsonSerializer.SerializeAsync(context.Response.Body, response, jsonOptions);
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var localizer = context.HttpContext.RequestServices.GetRequiredService<IJsonStringLocalizer>();
                        var message = localizer.GetString(MessageConstants.System.ForbiddenAccess);
                        var response = BaseAPIResponse<object>.Failure(message, StatusCodes.Status403Forbidden);

                        var jsonOptions = new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                        };

                        await System.Text.Json.JsonSerializer.SerializeAsync(context.Response.Body, response, jsonOptions);
                    }
                };
            });
        }

        public static void ConfigureAuthentication(this WebApplication app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
        }
    }
}
