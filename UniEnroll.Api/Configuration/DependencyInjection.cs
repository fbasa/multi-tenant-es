using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using UniEnroll.Api.Middleware;
using UniEnroll.Api.Support;
using UniEnroll.Application;
using UniEnroll.Application.Common;
using UniEnroll.Infrastructure.Common;
using UniEnroll.Infrastructure.Common.Abstractions;
using UniEnroll.Infrastructure.Common.Idempotency;
using UniEnroll.Infrastructure.EF;
using UniEnroll.Infrastructure.EF.Persistence.Idempotency;
using UniEnroll.Messaging;
using UniEnroll.Observability;
using UniEnroll.Reporting;

namespace UniEnroll.Api.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApiCore(this IServiceCollection services, IConfiguration config)
    {
        services.AddMemoryCache(); // for in-proc caching


        services.AddObservability(config)               //UniEnroll.Observability
                .AddInfrastructureCommon(config)        //UniEnroll.Infrastructure.Common  
                .AddInfrastructureEf(config);            //UniEnroll.Infrastructure.EF

        //Two IdempotencyStore implementation,
        //"In-memory" - dev/local or single-instance demos only (not multi-node safe; resets on restart).
        //"sql" - production (SQL/Redis/etc.), supports multi-instance and survives restarts.
        //Based on config value, register both, resolve only one.
        services.AddKeyedSingleton<IIdempotencyStore, InMemoryIdempotencyStore>("memory")       //UniEnroll.Infrastructure.Common
                .AddKeyedScoped<IIdempotencyStore, IdempotencyStore>("sql")                     //UniEnroll.Infrastructure.EF
                .AddScoped<IIdempotencyStore>(sp =>
                    sp.GetRequiredKeyedService<IIdempotencyStore>(
                        sp.GetRequiredService<IConfiguration>()["Idempotency:Provider"] ?? "memory"));

        // Controllers + JSON
        services.AddControllers()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        //UniEnroll.Api.Configuration
        services.AddApiVersioningExtensions()           // Versioning
                .AddProblemDetailsExtensions()          // RFC7807 + correlationId
                .AddCorsExtensions(config)              // CORS
                .AddSwaggerExtensions()                 // Swagger
                .AddAuthenticationExtensions(config)    // JWT token + Refresh token
                .AddAuthorizationExtensions()           // Authorization Policies
                .AddHealthChecksExtensins()             // HealthCheck
                .AddRateLimitingExtensions(config)      // Rate Limiting
                .AddDataProtectionExtensions(config)    // Data protection

        // Filters / behaviors (ModelState → ProblemDetails)
        .Configure<ApiBehaviorOptions>(o =>
        {
            o.InvalidModelStateResponseFactory = ctx => ProblemFactory.FromModelState(ctx);
        })

        // Helper for EF tenant filter
        .AddSingleton<EfTenantSetter>()

        // MediatR / Automapper / Pipeline behaviors i.e. ValidationBehavior,LoggingBehavior,TransactionBehavior,IdempotencyBehavior
        .AddApplication()   //UniEnroll.Application

        .AddMessaging(config)       //UniEnroll.Messaging

        .AddReporting(config);      //UniEnroll.Reporting

        return services;
    }
  
    public static IApplicationBuilder UseApiCore(this IApplicationBuilder app, bool dev)
    {
        app.UseCors();

        app.UseMiddleware<CorrelationIdMiddleware>();          // set correlationId very first
        app.UseMiddleware<ExceptionHandlingMiddleware>();      // <== NOW IT'S USED (wraps everything below)
        app.UseMiddleware<RequestLoggingMiddleware>();         // or your RequestLoggingMiddleware (pick one)

        app.UseRateLimiter();

        app.UseMiddleware<TenantResolutionMiddleware>();
        app.UseMiddleware<IdempotencyMiddleware>();
        app.UseExceptionHandler(_ => { }); // ProblemDetails wired via MapExceptions below
        app.MapProblemDetailsExceptions();  // our extension

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<DeviceFingerprintMiddleware>();

        if (dev)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
}
