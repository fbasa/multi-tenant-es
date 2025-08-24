
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using UniEnroll.Application.Abstractions;
using UniEnroll.Infrastructure.Common.Options;

namespace UniEnroll.Infrastructure.Common.Auth;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _opts;
    private readonly SigningCredentials _creds;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _opts = options.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.SigningKey ?? throw new InvalidOperationException("Jwt:SigningKey missing")));
        _creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    public string CreateAccessToken(string userId, string email, string tenantId, IEnumerable<string> roles, out DateTimeOffset expiresAt)
    {
        var now = DateTimeOffset.UtcNow;
        expiresAt = now.AddMinutes(_opts.AccessTokenMinutes);

        var claims = new List<Claim>
        {
            new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, userId),
            new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, email),
            new("tid", tenantId),
        };
        foreach (var r in roles) claims.Add(new(ClaimTypes.Role, r));

        var jwt = new JwtSecurityToken(
            issuer: _opts.Issuer,
            audience: _opts.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: _creds
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
