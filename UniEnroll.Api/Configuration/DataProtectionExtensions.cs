
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UniEnroll.Api.Configuration;

public static class DataProtectionExtensions
{
    public static IServiceCollection AddDataProtectionKeys(this IServiceCollection services, IConfiguration config)
    {
        var persistPath = config["DataProtection:PersistPath"];
        var b = services.AddDataProtection();
        if (!string.IsNullOrWhiteSpace(persistPath))
        {
            b.PersistKeysToFileSystem(new DirectoryInfo(persistPath));
        }
        return services;
    }
}
