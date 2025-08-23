using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UniEnroll.Infrastructure.Common.Idempotency;

namespace UniEnroll.Api.Middleware;

/// <summary>
/// Enforces idempotent POST/PUT with "Idempotency-Key" header; stores hash via IIdempotencyStore.
/// </summary>
public sealed class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<IdempotencyMiddleware> _logger;
    private readonly IIdempotencyStore _store;
    private readonly IOptions<IdempotencyOptions> _opts;

    public IdempotencyMiddleware(RequestDelegate next, ILogger<IdempotencyMiddleware> logger, IIdempotencyStore store, IOptions<IdempotencyOptions> opts)
    {
        _next = next; _logger = logger; _store = store; _opts = opts;
    }

    public async Task Invoke(HttpContext context)
    {
        if (HttpMethods.IsPost(context.Request.Method) || HttpMethods.IsPut(context.Request.Method))
        {
            if (!context.Request.Headers.TryGetValue("Idempotency-Key", out var key) || string.IsNullOrWhiteSpace(key))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Title = "Idempotency key required",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Provide 'Idempotency-Key' header for POST/PUT requests."
                });
                return;
            }

            var idKey = key.ToString();
            if (idKey.Length > _opts.Value.KeyMaxLength) idKey = idKey[.._opts.Value.KeyMaxLength];

            context.Request.EnableBuffering();
            string body;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
            { body = await reader.ReadToEndAsync(); context.Request.Body.Position = 0L; }

            var hash = ContentHasher.Sha256(body);
            var replay = await _store.CheckAndRecordAsync(idKey, hash, _opts.Value.TtlMinutes, context.RequestAborted);
            if (replay)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsJsonAsync(new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Title = "Duplicate request",
                    Status = StatusCodes.Status409Conflict,
                    Detail = "This request was already processed."
                });
                return;
            }
        }
        await _next(context);
    }
}
