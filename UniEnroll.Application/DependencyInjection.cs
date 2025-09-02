using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using UniEnroll.Application.Common;
using UniEnroll.Application.Common.Behaviors;
using UniEnroll.Application.Mapping;

namespace UniEnroll.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        })

        .AddSingleton(sp => new MapperConfiguration(config =>
        {
            config.AddProfile<ApplicationMappingConfig>();
        }, sp.GetRequiredService<ILoggerFactory>()).CreateMapper())

        // Pipeline behaviors (ordered)
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(IdempotencyBehavior<,>));

        return services;
    }
}
