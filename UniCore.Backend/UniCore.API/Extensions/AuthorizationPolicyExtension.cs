using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UniCore.API.AuthorizationHandler;
using UniCore.API.AuthorizationHandler.Permission;

namespace UniCore.API.Extensions
{
    public static class AuthorizationPolicyExtension
    {
        public static void AddAuthorizationPolicyService(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }

        public static void ConfigureAuthorization(this WebApplication app, IWebHostEnvironment env)
        {
            app.UseAuthorization();
        }
    }
}
