
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UniEnroll.Infrastructure.Common.Abstractions;

public sealed record RefreshIssueResult(string RefreshToken, DateTimeOffset ExpiresAt);
public sealed record RefreshRotateResult(
    bool Valid,
    string? UserId,
    string? TenantId,
    string[] Roles,
    string? NewRefreshToken,
    DateTimeOffset? NewRefreshExpiresAt,
    string? Error);

public interface IRefreshTokenService
{
    Task<RefreshIssueResult> IssueAsync(string tenantId, string userId, string? deviceId, string createdByIp, CancellationToken ct = default);
    Task<RefreshRotateResult> ValidateAndRotateAsync(string token, string? expectedTenantId, string? deviceId, string ip, CancellationToken ct = default);
    Task RevokeAsync(string token, string ip, string reason, CancellationToken ct = default);
    Task RevokeAllForUserAsync(string tenantId, string userId, string ip, string reason, CancellationToken ct = default);
}
