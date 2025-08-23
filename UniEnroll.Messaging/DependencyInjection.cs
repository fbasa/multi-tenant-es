
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniEnroll.Messaging.Abstractions;
using UniEnroll.Messaging.RabbitMq;
using UniEnroll.Messaging.Consumers;

namespace UniEnroll.Messaging;

public static class DependencyInjection
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<RabbitMqOptions>(config.GetSection("RabbitMQ"));

        services.AddSingleton<RabbitMqConnectionFactory>();
        services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();

        // Consumers
        services.AddSingleton<IEventConsumer, PaymentSucceededConsumer>();
        services.AddSingleton<IEventConsumer, EnrollmentConfirmedConsumer>();
        services.AddSingleton<IEventConsumer, InvoiceGeneratedConsumer>();
        services.AddSingleton<IEventConsumer, ReportingProjectionConsumer>();
        services.AddSingleton<IEventConsumer, StudentNotificationsConsumer>();

        // Background consumer
        services.AddHostedService<RabbitMqBackgroundConsumer>();

        return services;
    }
}
