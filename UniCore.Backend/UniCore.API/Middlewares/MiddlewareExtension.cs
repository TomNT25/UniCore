using System.Text.Json;
using UniCore.API.Extensions;
using UniCore.API.Middlewares.CustomMiddlewares;
using UniCore.Application.DTO;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Middlewares
{
    public static class MiddlewareExtension
    {
        public static void ConfigureMiddlewarePipeline(this WebApplication app, IWebHostEnvironment env)
        {
            // Diagnostics & Error Handling
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseExceptionHandler();
            app.UseRequestLocalization();

            // Handle empty body responses (e.g., 404 Not Found from unmapped routes, 401/403)
            app.UseStatusCodePages(async statusCodeContext =>
            {
                var httpContext = statusCodeContext.HttpContext;
                if (!httpContext.Response.HasStarted)
                {
                    httpContext.Response.ContentType = "application/json";
                    var localizer = httpContext.RequestServices.GetRequiredService<IJsonStringLocalizer>();
                    var statusCode = httpContext.Response.StatusCode;

                    string message = statusCode switch
                    {
                        StatusCodes.Status401Unauthorized => localizer.GetString(MessageConstants.System.UnauthorizedAccess),
                        StatusCodes.Status403Forbidden => localizer.GetString(MessageConstants.System.ForbiddenAccess),
                        StatusCodes.Status404NotFound => localizer.GetString(MessageConstants.System.ResourceNotFound),
                        _ => $"HTTP Error {statusCode}"
                    };

                    var response = BaseAPIResponse<object>.Failure(message, statusCode);

                    var jsonOptions = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    await JsonSerializer.SerializeAsync(httpContext.Response.Body, response, jsonOptions);
                }
            });

            // Security & Redirection
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.ConfigureCors(env);
            app.ConfigureForwardedHeadersOptions();
            app.ConfigureRateLimiting();

            // Routing & Authorization
            app.ConfigureAuthentication(env);
            app.ConfigureAuthorization(env);

            // Endpoints
            app.MapHealthChecks("/health");
            app.MapControllers();
        }
    }
}
