using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using UniEnroll.Application.Common.Behaviors;
using UniEnroll.Application.Mapping;

namespace UniEnroll.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //var asm = Assembly.GetExecutingAssembly();
        //services.AddMediatR(asm);
        //services.AddValidatorsFromAssembly(asm, includeInternalTypes: true);
        //services.AddAutoMapper(asm);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddSingleton(sp => new MapperConfiguration(config =>
        {
            config.AddProfile<ApplicationMappingConfig>();
        }, sp.GetRequiredService<ILoggerFactory>()).CreateMapper());

        // Pipeline behaviors (ordered)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(IdempotencyBehavior<,>));
        return services;
    }
}
