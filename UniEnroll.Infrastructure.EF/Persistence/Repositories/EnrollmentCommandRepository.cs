
using Microsoft.EntityFrameworkCore;
using System.Data;
using UniEnroll.Application.Abstractions;
using UniEnroll.Infrastructure.EF.Persistence.Sql;

namespace UniEnroll.Infrastructure.EF.Persistence.Repositories;

public sealed class EnrollmentCommandRepository : IEnrollmentCommandRepository
{
    private readonly UniEnrollDbContext _ctx;
    public EnrollmentCommandRepository(UniEnrollDbContext ctx) => _ctx = ctx;

    public async Task EnrollOrWaitlistAsync(string tenantId, string sectionId, string studentId, string enrollmentId, string? reason, CancellationToken ct = default)
    {
        using var trx = await _ctx.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        await EnrollmentSql.TryEnrollOrWaitlistAsync(_ctx, tenantId, sectionId, studentId, enrollmentId, reason, ct);
        await trx.CommitAsync(ct);
    }

    public async Task DropAsync(string tenantId, string enrollmentId, string sectionId, byte[] rowVersion, string actorUserId, string? reason, CancellationToken ct = default)
    {
        using var trx = await _ctx.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        await EnrollmentSql.DropAndPromoteAsync(_ctx, tenantId, enrollmentId, sectionId, rowVersion, actorUserId, reason, ct);
        await trx.CommitAsync(ct);
    }
}
