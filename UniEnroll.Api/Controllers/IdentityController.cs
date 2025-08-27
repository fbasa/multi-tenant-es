using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UniEnroll.Application.Features.Identity.Commands;
using UniEnroll.Application.Features.Identity.Queries;
using UniEnroll.Contracts.Identity;
using UniEnroll.Infrastructure.Common.Abstractions;
using UniEnroll.Infrastructure.Common.Options;

namespace UniEnroll.Api.Controllers;

public sealed class IdentityController : BaseApiController
{
    private readonly IAuthService _auth;
    private readonly IJwtTokenService _jwt;
    private readonly IRefreshTokenService _rt;
    private readonly ITenantContext _tenant;
    private readonly RefreshTokenOptions _rtOpts;

    public IdentityController(ISender sender, 
        IAuthService auth, 
        IJwtTokenService jwt, 
        IRefreshTokenService rt, 
        ITenantContext tenant, 
        IOptions<RefreshTokenOptions> rtOpts) : base(sender) 
    {
        _auth = auth; 
        _jwt = jwt; 
        _rt = rt; 
        _tenant = tenant; 
        _rtOpts = rtOpts.Value;
    }

    [AllowAnonymous]
    [HttpPost("{tenantId}/register")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromRoute] string tenantId, [FromBody] RegisterUserCommand body, CancellationToken ct)
        => Ok((await Sender.Send(body with { TenantId = tenantId }, ct)).Value);

    [HttpPost("{tenantId}/assign-role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignRole([FromRoute] string tenantId, [FromBody] AssignRoleCommand body, CancellationToken ct)
        => Ok(await Sender.Send(body with { TenantId = tenantId }, ct));

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthTokensResponse>> Login([FromBody] LoginRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(_tenant.TenantId))
            return Problem(statusCode: 400, title: "Tenant missing", detail: "Tenant header/subdomain not resolved.");

        var result = await _auth.AuthenticateAsync(_tenant.TenantId!, req.Email, req.Password, ct);
        if (result is null) return Unauthorized();

        var token = _jwt.CreateAccessToken(result.UserId, result.Email, result.TenantId, result.Roles, out var exp);
        return Ok(new AuthTokensResponse { AccessToken = token, RefreshToken = "", ExpiresAt = exp });
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthTokensResponse>> Refresh([FromBody] RefreshTokenRequest? req, CancellationToken ct)
    {
        string? raw = null;
        if (_rtOpts.UseCookies && Request.Cookies.TryGetValue(_rtOpts.CookieName, out var fromCookie))
            raw = fromCookie;
        if (string.IsNullOrWhiteSpace(raw))
            raw = req?.RefreshToken;
        if (string.IsNullOrWhiteSpace(raw))
            return Problem(statusCode: 400, title: "Missing refresh token");

        var device = Request.Headers.TryGetValue("X-Device-Id", out var d) ? d.ToString() : null;
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "-";
        var rotate = await _rt.ValidateAndRotateAsync(raw!, _tenant.TenantId, device, ip, ct);
        if (!rotate.Valid) return Unauthorized();

        // Rotate cookie
        if (_rtOpts.UseCookies && rotate.NewRefreshToken is not null && rotate.NewRefreshExpiresAt is not null)
        {
            Response.Cookies.Append(_rtOpts.CookieName, rotate.NewRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = rotate.NewRefreshExpiresAt.Value.UtcDateTime,
                IsEssential = true
            });
        }

        // New access token
        var access = _jwt.CreateAccessToken(rotate.UserId!, User.FindFirst("email")?.Value ?? "", rotate.TenantId!, rotate.Roles, out var exp);
        return Ok(new AuthTokensResponse { AccessToken = access, ExpiresAt = exp });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        // Revoke the current refresh token (if any)
        if (_rtOpts.UseCookies && Request.Cookies.TryGetValue(_rtOpts.CookieName, out var raw))
        {
            await _rt.RevokeAsync(raw, HttpContext.Connection.RemoteIpAddress?.ToString() ?? "-", "logout", ct);
            Response.Cookies.Delete(_rtOpts.CookieName, new CookieOptions { Secure = true, HttpOnly = true, SameSite = SameSiteMode.Strict });
        }
        return NoContent();
    }

    [HttpGet("me")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Me(CancellationToken ct) => Ok((await Sender.Send(new GetMeQuery(), ct)).Value);
}
