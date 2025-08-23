
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniEnroll.Seeder.Seeders;

namespace UniEnroll.Seeder;

internal interface IDataSeeder
{
    Task SeedAsync(CancellationToken ct);
}

public static class DependencyInjection
{
    public static IServiceCollection AddSeederServices(this IServiceCollection services, IConfiguration config)
    {
        // Seeders are registered in the desired execution order
        services.AddTransient<IDataSeeder, TenantSeeder>();
        services.AddTransient<IDataSeeder, RolesPermissionsSeeder>();
        services.AddTransient<IDataSeeder, AdminUserSeeder>();
        services.AddTransient<IDataSeeder, DemoAcademicDataSeeder>();

        // Allow seeders to access config + logging via DI
        services.AddSingleton(config);
        services.AddLogging();

        return services;
    }
}
