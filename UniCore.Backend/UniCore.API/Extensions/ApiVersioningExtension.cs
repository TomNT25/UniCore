using Asp.Versioning;
using UniCore.API.Context;

namespace UniCore.API.Extensions
{
    public static class ApiVersioningExtension
    {
        public static void AddApiVersioningService(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddScoped<IRequestContext, RequestContext>();
        }
    }
}
