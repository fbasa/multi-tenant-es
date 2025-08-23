
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace UniEnroll.Messaging.RabbitMq;

public sealed class RabbitMqConnectionFactory
{
    private readonly RabbitMqOptions _opts;
    private readonly ConnectionFactory _factory;

    public RabbitMqConnectionFactory(IOptions<RabbitMqOptions> opts)
    {
        _opts = opts.Value;
        _factory = new ConnectionFactory
        {
            HostName = _opts.HostName,
            Port = _opts.Port,
            VirtualHost = _opts.VirtualHost,
            UserName = _opts.UserName,
            Password = _opts.Password,
            //DispatchConsumersAsync = true
        };
    }

    public async Task<IConnection> CreateConnectionAsync() => await _factory.CreateConnectionAsync(default);
}
