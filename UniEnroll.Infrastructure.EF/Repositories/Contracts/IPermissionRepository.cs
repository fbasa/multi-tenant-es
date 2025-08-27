
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Domain.Identity;

namespace UniEnroll.Infrastructure.EF.Repositories.Contracts;

/// <summary>
/// Checks if a user has a given permission within a tenant.
/// </summary>
public interface IPermissionRepository
{
    Task<bool> HasPermissionAsync(string tenantId, string userId, string permission, CancellationToken ct = default);
}
