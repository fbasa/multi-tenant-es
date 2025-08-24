
using System.Threading;
using System.Threading.Tasks;

namespace UniEnroll.Application.Abstractions;

public sealed record AuthResult(string UserId, string Email, string TenantId, string[] Roles);

public interface IAuthService
{
    Task<AuthResult?> AuthenticateAsync(string tenantId, string email, string password, CancellationToken ct = default);
}
