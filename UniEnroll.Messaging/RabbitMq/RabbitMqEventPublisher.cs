
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using UniEnroll.Messaging.Abstractions;

namespace UniEnroll.Messaging.RabbitMq;

public sealed class RabbitMqEventPublisher : IEventPublisher, IDisposable
{
    private readonly RabbitMqOptions _opts;
    private readonly ILogger<RabbitMqEventPublisher> _logger;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    private IConnection? _conn;
    private IChannel? _ch;        // v7 replaces IModel with IChannel

    public RabbitMqEventPublisher(RabbitMqConnectionFactory cf, 
        IOptions<RabbitMqOptions> options, ILogger<RabbitMqEventPublisher> logger)
    {
        _opts = options.Value;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T @event, CancellationToken ct = default)
    {
        _ch = await EnsureChannelAsync(ct);

        var json = JsonSerializer.SerializeToUtf8Bytes(@event);
        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent,
        };

        props.Headers = new Dictionary<string, object?> { ["x-event-type"] = typeof(T).FullName ?? typeof(T).Name };

        await _ch.BasicPublishAsync(
                exchange: _opts.Exchange,
                routingKey: _opts.RoutingKey,
                mandatory: false,
                basicProperties: props,
                body: json,
                cancellationToken: ct);

        _logger.LogInformation("Published event {Type} with key {Key}", typeof(T).Name, _opts.RoutingKey);
    }

    private async Task<IChannel> EnsureChannelAsync(CancellationToken ct)
    {
        if (_ch is not null) return _ch;

        await _initLock.WaitAsync(ct);
        try
        {
            if (_ch is not null) return _ch;

            var factory = new ConnectionFactory
            {
                UserName = _opts.UserName,
                Password = _opts.Password,
                ClientProvidedName = "unienroll-api"
            };

            var endpoints = new[] { new AmqpTcpEndpoint(_opts.HostName, _opts.Port) };

            _conn =  await factory.CreateConnectionAsync(endpoints, factory.ClientProvidedName!, ct);

            // v7 requires CreateChannelOptions
            var chOptions = new CreateChannelOptions(publisherConfirmationsEnabled: true, publisherConfirmationTrackingEnabled: true);
            _ch = await _conn.CreateChannelAsync(chOptions, ct);

            // Topology
            await _ch.ExchangeDeclareAsync(_opts.Exchange, ExchangeType.Topic, durable: true, autoDelete: false, cancellationToken: ct);
            await _ch.QueueDeclareAsync("email.outgoing", durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: ct);
            await _ch.QueueBindAsync("email.outgoing", _opts.Exchange, _opts.RoutingKey, arguments: null, cancellationToken: ct);

            _logger.LogInformation("RabbitMQ email queue wired: {Host}:{Port}", _opts.HostName, _opts.Port);
            return _ch;
        }
        finally
        {
            _initLock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        try { if (_ch is not null) await _ch.DisposeAsync(); } catch { }
        try { if (_conn is not null) await _conn.DisposeAsync(); } catch { }
        _initLock.Dispose();
    }

    public void Dispose() => DisposeAsync().GetAwaiter().GetResult();
}
