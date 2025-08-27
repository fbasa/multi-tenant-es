
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UniEnroll.Infrastructure.Common.Abstractions;

namespace UniEnroll.Infrastructure.EF.Persistence.Interceptors;

public sealed class TenantWriteInterceptor : SaveChangesInterceptor
{
    private readonly ITenantContext _tenant;
    public TenantWriteInterceptor(ITenantContext tenant) => _tenant = tenant;

    public override InterceptionResult<int> SavingChanges(DbContextEventData e, InterceptionResult<int> result)
    {
        EnforceTenant(e.Context);
        return base.SavingChanges(e, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData e, InterceptionResult<int> result, CancellationToken ct = default)
    {
        EnforceTenant(e.Context);
        return base.SavingChangesAsync(e, result, ct);
    }

    private void EnforceTenant(DbContext? ctx)
    {
        if (ctx is null) return;
        var tenantId = _tenant.TenantId ?? throw new InvalidOperationException("Tenant not resolved");

        foreach (var entry in ctx.ChangeTracker.Entries().Where(x =>
                 x.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
        {
            var prop = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "TenantId");
            if (prop is null) continue;

            if (entry.State == EntityState.Added)
            {
                if (prop.CurrentValue is null) prop.CurrentValue = tenantId;
                if (!Equals(prop.CurrentValue, tenantId))
                    throw new InvalidOperationException("Cross-tenant insert blocked.");
            }
            else
            {
                if (!Equals(prop.OriginalValue, tenantId) || !Equals(prop.CurrentValue, tenantId))
                    throw new InvalidOperationException("Cross-tenant write blocked.");
            }
        }
    }
}