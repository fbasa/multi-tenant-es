
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UniEnroll.Seeder;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((ctx, cfg) =>
            {
                cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
                cfg.AddEnvironmentVariables("UNIENROLL_");
            })
            .ConfigureLogging(b =>
            {
                b.ClearProviders();
                b.AddSimpleConsole(o => { o.SingleLine = true; o.TimestampFormat = "HH:mm:ss "; });
                b.SetMinimumLevel(LogLevel.Information);
            })
            .ConfigureServices((ctx, services) =>
            {
                services.AddSeederServices(ctx.Configuration);
            })
            .Build();

        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        try
        {
            var seeders = host.Services.GetRequiredService<IEnumerable<IDataSeeder>>();
            foreach (var seeder in seeders)
            {
                logger.LogInformation("Running {Seeder}...", seeder.GetType().Name);
                await seeder.SeedAsync(CancellationToken.None);
            }
            logger.LogInformation("Seeding completed.");
            return 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Seeding failed");
            return 1;
        }
    }
}
