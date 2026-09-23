using System.Threading.RateLimiting;
using UniCore.Application.DTO;

namespace UniCore.API.Extensions
{
    public static class RateLimitingExtension
    {
        public static void AddRateLimitingService(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.OnRejected = async (context, cancellationToken) =>
                {
                    var response = context.HttpContext.Response;

                    response.StatusCode = StatusCodes.Status429TooManyRequests;

                    if (context.Lease.TryGetMetadata(
                        MetadataName.RetryAfter,
                        out var retryAfter))
                    {
                        response.Headers.RetryAfter =
                            ((int)retryAfter.TotalSeconds).ToString();
                    }

                    await context.HttpContext.Response.WriteAsJsonAsync(
                        new BaseAPIResponse<object>
                        {
                            StatusCode = 429,
                            Message = "Too many requests. Please try again later."
                        },
                        cancellationToken);
                };

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                    httpContext =>
                    {
                        var clientIp =
                            httpContext.Connection.RemoteIpAddress?.ToString()
                            ?? "unknown";

                        return RateLimitPartition.GetFixedWindowLimiter(
                            clientIp,
                            _ => new FixedWindowRateLimiterOptions
                            {
                                PermitLimit = 100,
                                Window = TimeSpan.FromMinutes(1),
                                QueueLimit = 0
                            });
                    });

                // Named policies
                options.AddPolicy("login", httpContext =>
                {
                    var clientIp =
                        httpContext.Connection.RemoteIpAddress?.ToString()
                        ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        clientIp,
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                });

                options.AddPolicy("expensive", httpContext =>
                {
                    var clientIp =
                        httpContext.Connection.RemoteIpAddress?.ToString()
                        ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        clientIp,
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                });
            });
        }

        public static void ConfigureRateLimiting(this WebApplication app)
        {
            app.UseRateLimiter();
        }
    }
}
