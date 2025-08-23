using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UniEnroll.Application.Abstractions;

namespace UniEnroll.Infrastructure.Common.Auth;

public sealed class HttpCurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _http;
    public HttpCurrentUser(IHttpContextAccessor http) => _http = http;

    public bool IsAuthenticated => _http.HttpContext?.User?.Identity?.IsAuthenticated == true;

    public string? UserId =>
        _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? _http.HttpContext?.User?.FindFirst("sub")?.Value;

    public string? Email =>
        _http.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

    public string[] Roles => _http.HttpContext?.User?.Claims
        .Where(c => c.Type == ClaimTypes.Role || c.Type == "role" || c.Type == "roles")
        .Select(c => c.Value).Distinct().ToArray() ?? System.Array.Empty<string>();

    public string? TenantId
        => _http.HttpContext?.Items.TryGetValue("TenantId", out var v) == true ? v?.ToString() : null;
}
