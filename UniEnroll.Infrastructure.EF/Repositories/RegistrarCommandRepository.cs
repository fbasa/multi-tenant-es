
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Features.Registrar.Commands.Common;
using UniEnroll.Contracts.Registrar;
using UniEnroll.Infrastructure.EF.Sql;

namespace UniEnroll.Infrastructure.EF.Repositories;

public sealed class RegistrarCommandRepository : IRegistrarCommandRepository
{
    private readonly string _cs;
    public RegistrarCommandRepository(IConfiguration config)
        => _cs = config.GetConnectionString("Sql") ?? config["Sql:ConnectionString"] ?? string.Empty;

    public async Task<UpsertTermResult> UpsertTermAsync(UpsertTermRequest request, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(RegistrarSql.UpsertTerm, conn);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier){ Value = request.TermId });
        cmd.Parameters.Add(new SqlParameter("@code", SqlDbType.NVarChar, 32){ Value = request.Code });
        cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 128){ Value = request.Name });
        cmd.Parameters.Add(new SqlParameter("@start", SqlDbType.Date){ Value = request.StartDate });
        cmd.Parameters.Add(new SqlParameter("@end", SqlDbType.Date){ Value = request.EndDate });
        var pRow = new SqlParameter("@rowver", SqlDbType.Timestamp){ Value = (object?)request.RowVersion ?? DBNull.Value };
        pRow.IsNullable = true;
        cmd.Parameters.Add(pRow);
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        if (!await rdr.ReadAsync(ct)) return new UpsertTermResult(RegistrarOutcome.Conflict);
        var outcome = rdr.GetString(0);
        return outcome switch
        {
            "Inserted" => new UpsertTermResult(RegistrarOutcome.Inserted),
            "Updated"  => new UpsertTermResult(RegistrarOutcome.Updated),
            "Conflict" => new UpsertTermResult(RegistrarOutcome.Conflict),
            _          => new UpsertTermResult(RegistrarOutcome.Conflict)
        };
    }

    public async Task<SetEnrollmentWindowResult> SetEnrollmentWindowAsync(SetEnrollmentWindowRequest request, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(RegistrarSql.SetEnrollmentWindow, conn);
        cmd.Parameters.Add(new SqlParameter("@term", SqlDbType.UniqueIdentifier){ Value = request.TermId });
        cmd.Parameters.Add(new SqlParameter("@startAt", SqlDbType.DateTimeOffset){ Value = request.StartAt });
        cmd.Parameters.Add(new SqlParameter("@endAt", SqlDbType.DateTimeOffset){ Value = request.EndAt });
        var pRow = new SqlParameter("@rowver", SqlDbType.Timestamp){ Value = (object?)request.RowVersion ?? DBNull.Value };
        pRow.IsNullable = true;
        cmd.Parameters.Add(pRow);
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        if (!await rdr.ReadAsync(ct)) return new SetEnrollmentWindowResult(RegistrarOutcome.Conflict);
        var outcome = rdr.GetString(0);
        return outcome switch
        {
            "Inserted"        => new SetEnrollmentWindowResult(RegistrarOutcome.Inserted),
            "Updated"         => new SetEnrollmentWindowResult(RegistrarOutcome.Updated),
            "NotFound"        => new SetEnrollmentWindowResult(RegistrarOutcome.NotFound),
            "ValidationFailed"=> new SetEnrollmentWindowResult(RegistrarOutcome.ValidationFailed),
            "Conflict"        => new SetEnrollmentWindowResult(RegistrarOutcome.Conflict),
            _                 => new SetEnrollmentWindowResult(RegistrarOutcome.Conflict)
        };
    }
}
