
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace UniEnroll.Observability.HealthChecks;

public sealed class RedisHealthCheck : IHealthCheck
{
    private readonly string _cs;
    public RedisHealthCheck(IConfiguration config)
        => _cs = config["Redis:ConnectionString"] ?? config.GetConnectionString("Redis") ?? "localhost:6379";

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var mux = await ConnectionMultiplexer.ConnectAsync(_cs);
            var server = mux.GetServer(mux.GetEndPoints()[0]);
            var pong = await mux.GetDatabase().PingAsync();
            return HealthCheckResult.Healthy($"Redis OK; ping={pong.TotalMilliseconds:0}ms, ver={server.Version}");
        }
        catch (System.Exception ex)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
        }
    }
}
