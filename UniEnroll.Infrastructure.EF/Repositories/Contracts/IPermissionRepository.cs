namespace UniEnroll.Infrastructure.EF.Repositories.Contracts;

public interface IPermissionRepository
{
    Task<bool> HasPermissionAsync(string tenantId, string userId, string permission, CancellationToken ct = default);
}