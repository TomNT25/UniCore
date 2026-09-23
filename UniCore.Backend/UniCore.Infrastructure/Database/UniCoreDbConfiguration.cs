using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniCore.Infrastructure.Database.Interceptors;
using UniCore.Infrastructure.Extension;

namespace UniCore.Infrastructure.Database
{
    public static class UniCoreDbConfiguration
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var dbOptions = new DatabaseOptions();
            var section = configuration.GetSection("Database");
            section.Bind(dbOptions);

            services.AddSingleton<AuditableEntitySaveChangesInterceptor>();
            services.AddSingleton<LoggingDbCommandInterceptor>();
            services.AddSingleton<LoggingDbConnectionInterceptor>();
            services.AddSingleton<LoggingDbTransactionInterceptor>();

            services.AddDbContextPool<UniCoreDbContext>((serviceProvider, options) =>
            {
                options.ApplyConfiguration(dbOptions);                

                if (dbOptions.EnableSensitiveDataLogging)
                {
                    options.EnableSensitiveDataLogging();
                }

                options.AddInterceptors(
                    serviceProvider.GetRequiredService<AuditableEntitySaveChangesInterceptor>(),
                    serviceProvider.GetRequiredService<LoggingDbCommandInterceptor>(),
                    serviceProvider.GetRequiredService<LoggingDbConnectionInterceptor>(),
                    serviceProvider.GetRequiredService<LoggingDbTransactionInterceptor>()
                );
            }, dbOptions.PoolSize);

            return services;
        }
    }
}
