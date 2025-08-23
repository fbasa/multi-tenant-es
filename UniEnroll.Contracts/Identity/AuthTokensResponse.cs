
using System;

namespace UniEnroll.Contracts.Identity;

public sealed record AuthTokensResponse(string AccessToken, string RefreshToken, DateTimeOffset ExpiresAt);
