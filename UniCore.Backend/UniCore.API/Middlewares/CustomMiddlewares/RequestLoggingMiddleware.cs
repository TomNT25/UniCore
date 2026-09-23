using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UniCore.API.Context;

namespace UniCore.API.Middlewares.CustomMiddlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IRequestContext requestContext)
        {
            var request = context.Request;
            _logger.LogInformation("========== Request Started ==========");
            _logger.LogInformation($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            _logger.LogInformation($"Method: {request.Method}");
            _logger.LogInformation($"Path: {request.Path}");
            _logger.LogInformation($"QueryString: {request.QueryString}");
            _logger.LogInformation($"Host: {request.Host}");
            _logger.LogInformation($"ContentType: {request.ContentType}");

            var stopwatch = Stopwatch.StartNew();

            if (context.Request.Headers.TryGetValue("X-Request-Id", out var requestId))
            {
                if (requestContext is RequestContext ctx)
                {
                    ctx.RequestId = requestId.ToString();
                }
            }

            context.Response.OnStarting(state =>
            {
                var httpContext = (HttpContext)state;
                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
                stopwatch.Stop();

                httpContext.Response.Headers.Append("X-Processing-Time-ms", stopwatch.ElapsedMilliseconds.ToString());                
                httpContext.Response.Headers.Append("X-Trace-Id", traceId);
                httpContext.Response.Headers.Append("X-Request-Id", requestContext.RequestId);

                return Task.CompletedTask;
            }, context);

            try
            {
                await _next(context);
            }
            finally
            {
                _logger.LogInformation($"Response Status Code: {context.Response.StatusCode}");
                _logger.LogInformation($"Request {context.Request.Method} {context.Request.Path} finished and processed in {stopwatch.ElapsedMilliseconds}ms");
                _logger.LogInformation("========== Request Completed ==========");
            }
        }
    }
}