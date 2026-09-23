using Microsoft.Extensions.Caching.Memory;
using UniCore.Application.Entity;

namespace UniCore.API.Middlewares.CustomMiddlewares
{
    public class TenantResolverMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TenantResolverMiddleware> _logger;
        private const string TenantHeaderKey = "X-Tenant-Id";


        public TenantResolverMiddleware(RequestDelegate next, ILogger<TenantResolverMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            ITenantSetter tenantSetter,
            IMemoryCache cache)
        {
            var path = context.Request.Path.Value ?? "";
            _logger.LogInformation("TenantResolverMiddleware: Processing path {Path}", path);

            // 1. Extract Tenant ID from Header (can also be parsed from Subdomain or JWT)
            var hasTenantHeader = context.Request.Headers.TryGetValue(TenantHeaderKey, out var tenantIdValue);
            var tenantId = tenantIdValue.ToString();

            _logger.LogInformation("TenantResolverMiddleware: HasHeader={HasHeader}, TenantId={TenantId}",
                hasTenantHeader, tenantId);

            if (!hasTenantHeader || string.IsNullOrWhiteSpace(tenantId))
            {
                // In a strict SaaS, we short-circuit if no tenant ID is provided
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Tenant identifier is missing in headers.");
                return;
            }

            // 2. Resolve Tenant details with caching to minimize DB/Redis overhead
            var cacheKey = $"tenant_info_{tenantId}";
            var tenant = await cache.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return ResolveTenantFromStore(tenantId!);
            });

            // 3. Validate tenant existence and account status
            if (tenant == null || !tenant.IsActive)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Invalid or inactive tenant account.");
                return;
            }

            // 4. Inject tenant into the scoped provider for downstream usage
            tenantSetter.CurrentTenant = tenant;
            _logger.LogInformation("TenantResolverMiddleware: Tenant {TenantId} resolved successfully", tenant.Id);

            await _next(context);
        }

        private async Task<Tenant?> ResolveTenantFromStore(string tenantId)
        {
            // Mocking a database lookup. Replace with your actual Repository or DB call.
            return await Task.FromResult(new Tenant
            {
                Id = tenantId,
                Name = $"Client {tenantId}",
                ConnectionString = $"Server=db_server;Database=db_{tenantId};User=sa;...",
                IsActive = true
            });
        }
    }
}
