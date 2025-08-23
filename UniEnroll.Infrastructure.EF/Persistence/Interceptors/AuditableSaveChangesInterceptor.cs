
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Infrastructure.EF.Persistence.Interceptors;

public sealed class AuditableSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var ctx = eventData.Context;
        if (ctx is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        foreach (var entry in ctx.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Modified) entry.Entity.Touch();
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
