
using System;
using System.Collections.Generic;

namespace UniEnroll.Application.Abstractions;

public interface IJwtTokenService
{
    string CreateAccessToken(string userId, string email, string tenantId, IEnumerable<string> roles, out DateTimeOffset expiresAt);
}
