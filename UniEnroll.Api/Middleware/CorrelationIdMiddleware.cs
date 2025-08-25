using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using UniEnroll.Infrastructure.Common.Logging;
using UniEnroll.Infrastructure.Common.Tenancy;

namespace UniEnroll.Api.Middleware;

public sealed class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next; _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var corr = context.Request.Headers.TryGetValue(TenantHeaderNames.CorrelationId, out var v) && !string.IsNullOrWhiteSpace(v)
            ? v.ToString()
            : System.Guid.NewGuid().ToString("N");

        context.Items[TenantHeaderNames.CorrelationId] = corr;
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[TenantHeaderNames.CorrelationId] = corr;
            return Task.CompletedTask;
        });

        SafeLogEnricher.SetCorrelationId(corr);

        using (_logger.BeginScope(new Dictionary<string, object> { ["correlationId"] = corr }))
        {
            var sw = Stopwatch.StartNew();
            await _next(context);
            sw.Stop();
            _logger.LogInformation("HTTP {Method} {Path} -> {StatusCode} in {Elapsed}ms",
                context.Request.Method, context.Request.Path, context.Response.StatusCode, sw.ElapsedMilliseconds);
        }
    }
}
