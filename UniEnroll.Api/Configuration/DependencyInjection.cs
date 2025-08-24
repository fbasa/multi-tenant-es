using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using UniEnroll.Api.Middleware;
using UniEnroll.Api.Support;
using UniEnroll.Application.Common;
using UniEnroll.Infrastructure.Common;
using UniEnroll.Infrastructure.EF;
using UniEnroll.Observability;

namespace UniEnroll.Api.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApiCore(this IServiceCollection services, IConfiguration config)
    {

        // 001 Serilog + Opentelemetry
        services.AddObservability(config)
                .AddInfrastructureCommon(config)
                .AddInfrastructureEf(config)

        // MediatR (scan Application assembly)
        .AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Result<>).Assembly);
        })

        // Controllers + JSON
        .AddControllers()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        services.AddApiVersioningExtensions()
                .AddProblemDetailsExtensions()          // RFC7807 + correlationId
                .AddCorsExtensions(config)
                .AddSwaggerExtensions()
                .AddAuthenticationExtensions(config)
                .AddAuthorizationExtensions()
                .AddHealthChecksExtensins()
                .AddRateLimitingExtensions(config)
                .AddDataProtectionExtensions(config)

        // Swagger
        .AddEndpointsApiExplorer()
        .AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "UniEnroll API", Version = "v1" });
        })

        // CORS
        .AddCors(opt =>
        {
            opt.AddDefaultPolicy(p => p
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins(config["Cors:Origins"]?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? new[] { "https://localhost" })
                .AllowCredentials());
        })


        // Filters / behaviors (ModelState → ProblemDetails)
        .Configure<ApiBehaviorOptions>(o =>
        {
            o.InvalidModelStateResponseFactory = ctx => ProblemFactory.FromModelState(ctx);
        })

        // Helper for EF tenant filter
        .AddSingleton<EfTenantSetter>();

        return services;
    }
  
    public static IApplicationBuilder UseApiCore(this IApplicationBuilder app, bool dev)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();          // set correlationId very first
        app.UseMiddleware<ExceptionHandlingMiddleware>();      // <== NOW IT'S USED (wraps everything below)
        app.UseMiddleware<RequestLoggingMiddleware>();         // or your RequestLoggingMiddleware (pick one)

        app.UseCors();
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
