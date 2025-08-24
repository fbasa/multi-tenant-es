
using System;

namespace UniEnroll.Contracts.Identity;

public sealed record AuthTokensResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}
