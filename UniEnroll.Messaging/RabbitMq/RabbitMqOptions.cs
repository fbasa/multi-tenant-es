
namespace UniEnroll.Messaging.RabbitMq;

public sealed class RabbitMqOptions
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string VirtualHost { get; set; } = "/";
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string Exchange { get; set; } = "unienroll.events";
    public string QueueNamePrefix { get; set; } = "unienroll.api";
    public bool Durable { get; set; } = true;
    public bool AutoDelete { get; set; } = false;
    public ushort Prefetch { get; set; } = 20;
    public string RoutingKey { get; set; }
}
