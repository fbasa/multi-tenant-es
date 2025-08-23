
using Microsoft.AspNetCore.Builder;

namespace UniEnroll.Api.Observability;

public static class SerilogRequestLogging
{
    public static IApplicationBuilder UseSerilogRequestLoggingIfAvailable(this IApplicationBuilder app)
    {
        try
        {
            // Will no-op if Serilog middleware not referenced at runtime
            var mi = typeof(ApplicationBuilderExtensions).GetMethod("UseSerilogRequestLogging", new[] { typeof(IApplicationBuilder) });
            mi?.Invoke(null, new object[] { app });
        }
        catch { /* ignore */ }
        return app;
    }
}
