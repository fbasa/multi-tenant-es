
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;
using UniEnroll.Domain.Abstractions;
using UniEnroll.Infrastructure.Common.Abstractions;
using UniEnroll.Infrastructure.EF.Persistence.Outbox;

namespace UniEnroll.Infrastructure.EF.Persistence.Interceptors;

public sealed class DispatchDomainEventsInterceptor(
        ITenantContext tenant,
        IHttpContextAccessor http,
        ILogger<DispatchDomainEventsInterceptor> log) : SaveChangesInterceptor
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        PersistOutbox(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        PersistOutbox(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void PersistOutbox(DbContext? ctx)
    {
        if (ctx is null) return;

        var domainEvents = DequeueDomainEvents(ctx.ChangeTracker);
        if (domainEvents.Count == 0) return;

        var set = ctx.Set<OutboxMessage>();
        var tenantId = tenant.TenantId;
        var correlationId = (http.HttpContext?.Request.Headers.TryGetValue("X-Correlation-Id", out var cid) == true && !string.IsNullOrWhiteSpace(cid))
            ? cid.ToString()
            : http.HttpContext?.TraceIdentifier;

        foreach (var de in domainEvents)
        {
            var type = de.GetType();
            var payload = JsonSerializer.Serialize(de, type, JsonOpts);
            var occurred = GetOccurredOn(de) ?? DateTimeOffset.UtcNow;

            var msg = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = type.FullName ?? type.Name,
                Payload = payload,
                TenantId = tenantId,
                CorrelationId = correlationId,
                OccurredAt = occurred,
                CreatedAt = DateTimeOffset.UtcNow,
                ProcessedAt = null,
                Error = null
            };

            set.Add(msg);
            log.LogDebug("Queued outbox message {MessageId} for event {EventType} (tenant={Tenant}, corr={CorrelationId})",
                msg.Id, msg.Type, tenantId ?? "-", correlationId ?? "-");
        }
    }

    /// <summary>
    /// Pulls domain events off tracked aggregates and clears them to avoid re-dispatch.
    /// Works with common patterns: DequeueDomainEvents(), PullDomainEvents(), or 'DomainEvents' list field/property.
    /// </summary>
    private static List<DomainEvent> DequeueDomainEvents(ChangeTracker tracker)
    {
        var all = new List<DomainEvent>(capacity: 16);

        foreach (var entry in tracker.Entries().Where(e => e.Entity is not null))
        {
            var entity = entry.Entity;
            var t = entity.GetType();

            // Preferred: method DequeueDomainEvents(): IReadOnlyCollection<DomainEvent>
            var mDequeue = t.GetMethod("DequeueDomainEvents", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Array.Empty<Type>());
            if (mDequeue is not null && typeof(IEnumerable<DomainEvent>).IsAssignableFrom(mDequeue.ReturnType))
            {
                if (mDequeue.Invoke(entity, null) is IEnumerable<DomainEvent> evs1)
                    all.AddRange(evs1);
                continue;
            }

            // Alternate: method PullDomainEvents()
            var mPull = t.GetMethod("PullDomainEvents", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Array.Empty<Type>());
            if (mPull is not null && typeof(IEnumerable<DomainEvent>).IsAssignableFrom(mPull.ReturnType))
            {
                if (mPull.Invoke(entity, null) is IEnumerable<DomainEvent> evs2)
                    all.AddRange(evs2);
                continue;
            }

            // Fallback: property/field 'DomainEvents'
            var p = t.GetProperty("DomainEvents", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            IEnumerable<DomainEvent>? evs = null;
            if (p?.GetValue(entity) is IEnumerable<DomainEvent> propEvs) evs = propEvs;

            var f = t.GetField("_domainEvents", BindingFlags.Instance | BindingFlags.NonPublic);
            if (evs is null && f?.GetValue(entity) is IEnumerable<DomainEvent> fieldEvs) evs = fieldEvs;

            if (evs is not null)
            {
                all.AddRange(evs);

                // try to clear
                if (p?.GetValue(entity) is IList<DomainEvent> listProp) listProp.Clear();
                else if (f?.GetValue(entity) is IList<DomainEvent> listField) listField.Clear();
                else
                {
                    // try ClearDomainEvents()
                    var mClear = t.GetMethod("ClearDomainEvents", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    mClear?.Invoke(entity, null);
                }
            }
        }

        return all;
    }

    private static DateTimeOffset? GetOccurredOn(DomainEvent de)
    {
        // If your DomainEvent base has OccurredOn/OccurredAt, use it. Otherwise null.
        var t = de.GetType();
        var p1 = t.GetProperty("OccurredOn") ?? t.GetProperty("OccurredAt");
        if (p1?.GetValue(de) is DateTimeOffset dto) return dto;
        if (p1?.GetValue(de) is DateTime dt) return new DateTimeOffset(dt, TimeSpan.Zero);
        return null;
    }
}
