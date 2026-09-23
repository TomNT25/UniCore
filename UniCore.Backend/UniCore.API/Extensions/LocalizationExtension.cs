using Microsoft.AspNetCore.Localization;
using System.Globalization;
using UniCore.Helper.Localization;

namespace UniCore.API.Extensions
{
    public static class LocalizationExtension
    {
        public static IServiceCollection AddBackendLocalizationServices(this IServiceCollection services)
        {
            services.AddSingleton<IJsonStringLocalizer, JsonStringLocalizer>();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("vi-VN")
                };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new AcceptLanguageHeaderRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });

            return services;
        }

        public static IApplicationBuilder UseBackendLocalization(this IApplicationBuilder app)
        {
            app.UseRequestLocalization();
            return app;
        }
    }
}
