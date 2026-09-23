using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UniCore.Application.Contract.Cache;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.UnitOfWork;
using UniCore.Helper.Constant;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.RequestHandlerHub;
using UniCore.Infrastructure.Util.Cache;

namespace UniCore.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(InfrastructureDependencyInjection).Assembly;

        services.AddTransient<IDispatcher, NativeDispatcher>();

        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes
                .Where(type => type.Name.Contains("Repository") || 
                               type.Name.Contains("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Redis Configuration
        var redisConn = configuration[CacheConstants.RedisConnectionStringPath];
        if (!string.IsNullOrWhiteSpace(redisConn))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConn;
                options.InstanceName = configuration[CacheConstants.RedisInstanceNamePath] ?? CacheConstants.DefaultInstanceName;
            });
            services.AddScoped<ICacheService, RedisCacheService>();
        }
        else
        {
            services.AddScoped<ICacheService, MemoryCacheService>();
        }

        services.AddDatabaseConfiguration(configuration);

        services.AddScoped(sp => new DatabaseConfiguration(DatabaseConfigurationConstant.SQL_SERVER_CONNECTION_STRING));

        var config = TypeAdapterConfig.GlobalSettings;
        var applicationAssembly = Assembly.Load("UniCore.Application");
        config.Scan(assembly, applicationAssembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        var healthChecks = services.AddHealthChecks().AddDbContextCheck<UniCoreDbContext>();
        if (!string.IsNullOrWhiteSpace(redisConn))
        {
            healthChecks.AddRedis(redisConn, name: CacheConstants.HealthCheckName);
        }

        return services;
    }
}