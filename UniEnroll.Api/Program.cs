using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using UniEnroll.Api.Configuration;
using UniEnroll.Api.Middleware;
using UniEnroll.Api.Security;
using UniEnroll.Api.Support;
using UniEnroll.Application.Common;
using UniEnroll.Infrastructure.Common;
using UniEnroll.Infrastructure.EF;
using UniEnroll.Observability;

var builder = WebApplication.CreateBuilder(args);

// 001 Serilog + Opentelemetry
builder.Services.AddObservability(builder.Configuration);
builder.Services.AddInfrastructureCommon(builder.Configuration);
builder.Services.AddInfrastructureEf(builder.Configuration);

// MediatR (scan Application assembly)
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Result<>).Assembly);
});

// Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// API versioning
builder.Services.AddApiVersioningV1();

// ProblemDetails (RFC 7807)
builder.Services.AddProblemDetails();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UniEnroll API", Version = "v1" });
});

// CORS
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p => p
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins(builder.Configuration["Cors:Origins"]?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? new[] { "https://localhost" })
        .AllowCredentials());
});

// Authentication/Authorization (JWT optional stub)
JwtAuthExtensions.AddJwt(builder.Services, builder.Configuration);

// Filters / behaviors (ModelState → ProblemDetails)
builder.Services.Configure<ApiBehaviorOptions>(o =>
{
    o.InvalidModelStateResponseFactory = ctx => ProblemFactory.FromModelState(ctx);
});

// Helper for EF tenant filter
builder.Services.AddSingleton<EfTenantSetter>();

var app = builder.Build();

// Middleware pipeline
app.UseCors();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<TenantResolutionMiddleware>();
app.UseMiddleware<IdempotencyMiddleware>();
app.UseExceptionHandler(_ => { }); // ProblemDetails wired via MapExceptions below
app.MapProblemDetailsExceptions();  // our extension

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

