using Microsoft.AspNetCore.Mvc;
using UniCore.Application.Contract.External;
using UniCore.Application.DTO;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;
using UniCore.Infrastructure.External;

namespace UniCore.API.Extensions
{
    public static class APIDependencyInjectionExtension
    {
        public static void AddAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioningService();

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var localizer = context.HttpContext.RequestServices.GetRequiredService<IJsonStringLocalizer>();
                        var errors = context.ModelState
                            .Where(x => x.Value?.Errors.Count > 0)
                            .SelectMany(x => x.Value!.Errors.Select(e =>
                            {
                                var msg = string.IsNullOrEmpty(e.ErrorMessage) ? "Invalid input parameter" : e.ErrorMessage;
                                return localizer.GetString(msg);
                            }))
                            .ToList();

                        var message = localizer.GetString(MessageConstants.System.ValidationFailed);
                        var response = BaseAPIResponse<object>.Failure(message, StatusCodes.Status400BadRequest, errors);

                        return new BadRequestObjectResult(response);
                    };
                });

            services.AddOpenApi();

            services.AddSwaggerGenService();

            services.AddHealthChecks();

            services.AddAuthenticationPolicyService(configuration);

            services.AddAuthorizationPolicyService();

            services.AddCorsService();

            services.AddBackendLocalizationServices();

            services.AddExceptionHandler<GlobalExceptionExtension>();

            services.AddMemoryCache();

            services.AddForwardedHeadersOptionsService();

            services.AddRateLimitingService();

            services.AddProblemDetails();

            services.AddHttpClient<IGoogleAuthProviderClient, GoogleAuthProviderClient>(client =>
            {
                var baseUrl = configuration["GoogleAuthProvider:BaseUrl"] ?? "http://localhost:5005";
                client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
            });
        }
    }
}
