
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Features.Payments.Queries.Common;
using UniEnroll.Infrastructure.EF.Sql;

namespace UniEnroll.Infrastructure.EF.Repositories;

public sealed class PaymentQueryRepository : IPaymentQueryRepository
{
    private readonly string _cs;
    public PaymentQueryRepository(IConfiguration cfg)
        => _cs = cfg.GetConnectionString("Sql") ?? cfg["Sql:ConnectionString"] ?? string.Empty;

    public async Task<PaymentStatusResult?> GetStatusAsync(Guid paymentId, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(PaymentSql.GetStatus, conn);
        cmd.Parameters.Add(new SqlParameter("@payment", SqlDbType.UniqueIdentifier){ Value = paymentId });
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        if (!await rdr.ReadAsync(ct)) return null;
        return new PaymentStatusResult(
            PaymentId: rdr.GetString(0),
            Status: rdr.GetString(1),
            Amount: rdr.GetDecimal(2),
            Currency: rdr.GetString(3)
        );
    }
}
