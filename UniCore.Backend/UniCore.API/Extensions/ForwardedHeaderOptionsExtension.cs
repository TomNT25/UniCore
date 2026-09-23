using Microsoft.AspNetCore.HttpOverrides;

namespace UniCore.API.Extensions
{
    public static class ForwardedHeadersOptionsExtension
    {
        public static void AddForwardedHeadersOptionsService(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedProto;
            });
        }

        public static void ConfigureForwardedHeadersOptions(this WebApplication app)
        {
            app.UseForwardedHeaders();
        }
    }
}
