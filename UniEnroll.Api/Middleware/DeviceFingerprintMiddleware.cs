
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UniEnroll.Api.Middleware;

public sealed class DeviceFingerprintMiddleware
{
    private readonly RequestDelegate _next;
    public const string CookieName = "ufp";
    public DeviceFingerprintMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx)
    {
        if (!ctx.Request.Cookies.TryGetValue(CookieName, out var fp) || string.IsNullOrWhiteSpace(fp))
        {
            var ua = ctx.Request.Headers["User-Agent"].ToString();
            var ip = ctx.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
            using var sha = SHA256.Create();
            fp = Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes($"{ua}|{ip}")));
            ctx.Response.Cookies.Append(CookieName, fp, new CookieOptions { HttpOnly = true, Secure = ctx.Request.IsHttps, SameSite = SameSiteMode.Lax, IsEssential = true });
        }
        ctx.Items["DeviceFingerprint"] = fp;
        await _next(ctx);
    }
}
