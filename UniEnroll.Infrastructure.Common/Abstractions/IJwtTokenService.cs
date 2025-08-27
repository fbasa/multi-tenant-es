
using System;
using System.Collections.Generic;

namespace UniEnroll.Infrastructure.Common.Abstractions;

public interface IJwtTokenService
{
    string CreateAccessToken(string userId, string email, string tenantId, IEnumerable<string> roles, out DateTimeOffset expiresAt);
}
