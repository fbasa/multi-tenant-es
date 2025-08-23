
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace UniEnroll.Observability.HealthChecks;

public sealed class RabbitMqHealthCheck : IHealthCheck
{
    private readonly IConfiguration _config;
    public RabbitMqHealthCheck(IConfiguration config) => _config = config;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        try
        {
            var host = _config["RabbitMQ:HostName"] ?? "localhost";
            var user = _config["RabbitMQ:UserName"] ?? "guest";
            var pass = _config["RabbitMQ:Password"] ?? "guest";
            var vhost = _config["RabbitMQ:VirtualHost"] ?? "/";
            var portStr = _config["RabbitMQ:Port"];
            var port = string.IsNullOrWhiteSpace(portStr) ? 5672 : int.Parse(portStr);

            var factory = new ConnectionFactory
            {
                HostName = host, UserName = user, Password = pass, VirtualHost = vhost, Port = port
            };
            var endpoints = new[] { new AmqpTcpEndpoint(host, port) };
            await using var conn = await factory.CreateConnectionAsync(endpoints, factory.ClientProvidedName!, ct);

            var chOptions = new CreateChannelOptions(publisherConfirmationsEnabled: true, publisherConfirmationTrackingEnabled: true);
            await using var ch = await conn.CreateChannelAsync(chOptions, ct);

            return HealthCheckResult.Healthy("RabbitMQ OK");
        }
        catch (System.Exception ex)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
        }
    }
}
