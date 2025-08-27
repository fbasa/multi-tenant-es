
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Domain.Abstractions;
using UniEnroll.Domain.Identity;
using UniEnroll.Infrastructure.Common.Abstractions;

namespace UniEnroll.Infrastructure.EF.Persistence.Interceptors;

public sealed class AuditableSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _clock;
    private readonly ICurrentUser _user;

    public AuditableSaveChangesInterceptor(IDateTimeProvider clock, ICurrentUser user)
        => (_clock, _user) = (clock, user);
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken ct = default)
    {
        var now = _clock.UtcNow;
        var userId = _user.UserId ?? "system";
        var ctx = eventData.Context;
        if (ctx is null) return base.SavingChangesAsync(eventData, result, ct);

        foreach (var entry in ctx.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Touch();
            }

            if (entry.State == EntityState.Added )
            {
                entry.Property("CreatedBy").CurrentValue = userId;
                entry.Property("UpdatedBy").CurrentValue = userId;
                entry.Property("CreatedAt").CurrentValue = now;    
                entry.Property("UpdatedAt").CurrentValue = now;  
            }
                
            if (entry.State == EntityState.Deleted && entry.Metadata.ClrType.GetInterface(nameof(ISoftDelete)) != null)
            {
                entry.State = EntityState.Modified;
                entry.Property("IsDeleted").CurrentValue = true;
                entry.Property("DeletedAt").CurrentValue = now;     
                entry.Property("DeletedBy").CurrentValue = userId; 
            }
        }
        return base.SavingChangesAsync(eventData, result, ct);
    }
}
