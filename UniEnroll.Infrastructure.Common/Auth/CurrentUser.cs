
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UniEnroll.Application.Abstractions;

namespace UniEnroll.Infrastructure.Common.Auth;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _http;
    public CurrentUser(IHttpContextAccessor http) => _http = http;

    public string? UserId =>
    _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
    ?? _http.HttpContext?.User?.FindFirst("sub")?.Value;

    public string? Email =>
        _http.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

    public string[] Roles =>
        _http.HttpContext?.User?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray()
        ?? Array.Empty<string>();

    public bool IsAuthenticated => throw new NotImplementedException(); //TODO:

    public string? TenantId => throw new NotImplementedException();
}
