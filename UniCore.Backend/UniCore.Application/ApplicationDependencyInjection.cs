using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UniCore.Application.Entity;

namespace UniCore.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationDependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(assembly);

        // Register handlers as self (so domain services can inject concrete handlers) & implemented interfaces
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Handler")))
            .AsSelf()
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Register Domain Services (e.g. AuthService, PermissionService, RoleService, UserService)
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<TenantProvider>();
        services.AddScoped<ITenantSetter>(sp => sp.GetRequiredService<TenantProvider>());
        services.AddScoped<ITenantGetter>(sp => sp.GetRequiredService<TenantProvider>());

        return services;
    }
}