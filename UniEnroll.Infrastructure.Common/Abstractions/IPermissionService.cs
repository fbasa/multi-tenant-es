
using System.Threading;
using System.Threading.Tasks;

namespace UniEnroll.Infrastructure.Common.Auth
{
    /// <summary>
    /// Checks if a user has a given permission within a tenant.
    /// </summary>
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(string tenantId, string userId, string permission, CancellationToken ct = default);
    }
}
