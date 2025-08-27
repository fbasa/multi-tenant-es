
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UniEnroll.Contracts.Payments;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;
using UniEnroll.Infrastructure.EF.Sql;

namespace UniEnroll.Infrastructure.EF.Repositories;

public sealed class PaymentCommandRepository : IPaymentCommandRepository
{
    private readonly string _cs;
    public PaymentCommandRepository(IConfiguration cfg)
        => _cs = cfg.GetConnectionString("Sql") ?? cfg["Sql:ConnectionString"] ?? string.Empty;

    public async Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest request, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);

        await using var cmd = new SqlCommand(PaymentSql.Capture, conn);

        cmd.Parameters.Add(new SqlParameter("@invoice", SqlDbType.UniqueIdentifier){ Value = request.InvoiceId });
        cmd.Parameters.Add(new SqlParameter("@amount", SqlDbType.Decimal){ Precision = 18, Scale = 2, Value = request.Amount });
        cmd.Parameters.Add(new SqlParameter("@currency", SqlDbType.NVarChar, 3){ Value = request.Currency });
        cmd.Parameters.Add(new SqlParameter("@method", SqlDbType.NVarChar, 32){ Value = request.Method });
        cmd.Parameters.Add(new SqlParameter("@gatewayTxnId", SqlDbType.NVarChar, 128){ Value = (object?)request.GatewayTxnId ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@idemKey", SqlDbType.NVarChar, 64){ Value = (object?)request.IdempotencyKey ?? DBNull.Value });
        
        await using var rdr = await cmd.ExecuteReaderAsync(ct);

        if (!await rdr.ReadAsync(ct))
        {
            return new CapturePaymentResult(PaymentOutcome.Conflict, null);
        }
            
        var outcome = rdr.GetString(0);
        var pid = rdr.IsDBNull(1) ? null : rdr.GetString(1);

        return outcome switch
        {
            "Captured"        => new CapturePaymentResult(PaymentOutcome.Captured, pid),
            "NotFound"        => new CapturePaymentResult(PaymentOutcome.NotFound, null),
            "AlreadyCaptured" => new CapturePaymentResult(PaymentOutcome.AlreadyCaptured, pid),
            _                 => new CapturePaymentResult(PaymentOutcome.Conflict, null)
        };
    }

    public async Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest request, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(PaymentSql.Refund, conn);
        cmd.Parameters.Add(new SqlParameter("@payment", SqlDbType.UniqueIdentifier){ Value = request.PaymentId });
        cmd.Parameters.Add(new SqlParameter("@amount", SqlDbType.Decimal){ Precision = 18, Scale = 2, Value = request.Amount });
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        if (!await rdr.ReadAsync(ct)) return new RefundPaymentResult(PaymentOutcome.Conflict, null);
        var outcome = rdr.GetString(0);
        var rid = rdr.IsDBNull(1) ? null : rdr.GetString(1);
        return outcome switch
        {
            "Refunded" => new RefundPaymentResult(PaymentOutcome.Refunded, rid),
            "NotFound" => new RefundPaymentResult(PaymentOutcome.NotFound, null),
            _          => new RefundPaymentResult(PaymentOutcome.Conflict, null)
        };
    }
}
