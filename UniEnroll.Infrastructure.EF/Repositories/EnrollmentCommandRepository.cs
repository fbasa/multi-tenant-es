
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Features.Enrollment.Commands.Common;
using UniEnroll.Infrastructure.EF.Sql;

namespace UniEnroll.Infrastructure.EF.Repositories;

public sealed class EnrollmentCommandRepository : IEnrollmentCommandRepository
{
    private readonly string _cs;
    public EnrollmentCommandRepository(IConfiguration config)
        => _cs = config.GetConnectionString("Sql") ?? config["Sql:ConnectionString"] ?? string.Empty;

    public async Task<ReserveSeatResult> ReserveSeatAsync(Guid sectionId, string studentId, string? idempotencyKey, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(EnrollmentSql.ReserveSeat, conn);
        cmd.Parameters.Add(new SqlParameter("@section", SqlDbType.UniqueIdentifier){ Value = sectionId });
        cmd.Parameters.Add(new SqlParameter("@student", SqlDbType.NVarChar, 64){ Value = studentId });
        cmd.Parameters.Add(new SqlParameter("@ttlMinutes", SqlDbType.Int){ Value = 15 });
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        if (!await rdr.ReadAsync(ct)) return new ReserveSeatResult(EnrollmentOutcome.Conflict, null, null);

        var outcome = rdr.GetString(0);
        return outcome switch
        {
            "Enrolled" => new ReserveSeatResult(EnrollmentOutcome.Enrolled, rdr.IsDBNull(1) ? null : rdr.GetFieldValue<Guid?>(1), rdr.IsDBNull(2) ? null : rdr.GetDateTimeOffset(2)),
            "Waitlisted" => new ReserveSeatResult(EnrollmentOutcome.Waitlisted, null, null),
            "NoSeats" => new ReserveSeatResult(EnrollmentOutcome.NoSeats, null, null),
            "AlreadyEnrolled" => new ReserveSeatResult(EnrollmentOutcome.AlreadyEnrolled, null, null),
            "NotFound" => new ReserveSeatResult(EnrollmentOutcome.NotFound, null, null),
            _ => new ReserveSeatResult(EnrollmentOutcome.Conflict, null, null)
        };
    }

    public async Task<EnrollSeatResult> EnrollAsync(Guid sectionId, string studentId, string? idempotencyKey, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(EnrollmentSql.Enroll, conn);
        cmd.Parameters.Add(new SqlParameter("@section", SqlDbType.UniqueIdentifier){ Value = sectionId });
        cmd.Parameters.Add(new SqlParameter("@student", SqlDbType.NVarChar, 64){ Value = studentId });
        cmd.Parameters.Add(new SqlParameter("@userId", SqlDbType.NVarChar, 64){ Value = studentId });
        cmd.Parameters.Add(new SqlParameter("@reason", SqlDbType.NVarChar, 128){ Value = "Enroll via API" });
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        if (!await rdr.ReadAsync(ct)) return new EnrollSeatResult(EnrollmentOutcome.Conflict, null);

        var outcome = rdr.GetString(0);
        return outcome switch
        {
            "Enrolled" => new EnrollSeatResult(EnrollmentOutcome.Enrolled, rdr.IsDBNull(1) ? null : rdr.GetGuid(1).ToString()),
            "AlreadyEnrolled" => new EnrollSeatResult(EnrollmentOutcome.AlreadyEnrolled, null),
            "NoSeats" => new EnrollSeatResult(EnrollmentOutcome.NoSeats, null),
            _ => new EnrollSeatResult(EnrollmentOutcome.Conflict, null)
        };
    }

    public async Task<DropResult> DropAsync(Guid enrollmentId, string? idempotencyKey, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(EnrollmentSql.Drop, conn);
        cmd.Parameters.Add(new SqlParameter("@enrollmentId", SqlDbType.UniqueIdentifier){ Value = enrollmentId });
        cmd.Parameters.Add(new SqlParameter("@userId", SqlDbType.NVarChar, 64){ Value = "system" });
        cmd.Parameters.Add(new SqlParameter("@reason", SqlDbType.NVarChar, 128){ Value = "Drop via API" });
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        if (!await rdr.ReadAsync(ct)) return new DropResult(EnrollmentOutcome.Conflict, false);

        var outcome = rdr.GetString(0);
        var promoted = rdr.GetBoolean(1);
        return outcome switch
        {
            "Enrolled" => new DropResult(EnrollmentOutcome.Enrolled, promoted),
            "NotFound" => new DropResult(EnrollmentOutcome.NotFound, false),
            "Conflict" => new DropResult(EnrollmentOutcome.Conflict, false),
            _ => new DropResult(EnrollmentOutcome.Conflict, false)
        };
    }
}
