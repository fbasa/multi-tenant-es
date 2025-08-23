
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace UniEnroll.Api.Auth;

public interface ICurrentUserAccessor
{
    string? UserId { get; }
    string[] Roles { get; }
    string? Email { get; }
}

public sealed class CurrentUserAccessor : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _http;
    public CurrentUserAccessor(IHttpContextAccessor http) => _http = http;
    public string? UserId => _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? _http.HttpContext?.User?.FindFirstValue("sub");
    public string[] Roles => _http.HttpContext?.User?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray() ?? System.Array.Empty<string>();
    public string? Email => _http.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
}
