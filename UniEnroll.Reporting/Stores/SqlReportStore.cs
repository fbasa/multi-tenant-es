
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UniEnroll.Reporting.Abstractions;
using UniEnroll.Contracts.Reporting;
using UniEnroll.Contracts.Common;
using Microsoft.Extensions.Configuration;

namespace UniEnroll.Reporting.Stores;

/// <summary>
/// Lightweight ADO.NET store executing parameterized, set-based SQL to avoid N+1 and support covering indexes.
/// </summary>
public sealed class SqlReportStore : IReportStore
{
    private readonly string _cs;
    public SqlReportStore(IConfiguration config)
    {
        _cs = config.GetConnectionString("Sql") ?? config["Sql:ConnectionString"] ?? string.Empty;
    }

    public async Task<IReadOnlyList<EnrollmentReportRowDto>> GetEnrollmentReportAsync(string tenantId, string? termId, CancellationToken ct = default)
    {
        const string sql = @"
SELECT e.TermId, s.CourseId, e.SectionId, COUNT_BIG(1) AS Cnt
FROM Enrollments e WITH (NOLOCK)
JOIN Sections s WITH (NOLOCK) ON e.SectionId = s.Id AND e.TenantId = s.TenantId
WHERE e.TenantId = @tenant AND (@term IS NULL OR e.TermId = @term)
GROUP BY e.TermId, s.CourseId, e.SectionId
ORDER BY e.TermId, s.CourseId, e.SectionId;";
        var list = new List<EnrollmentReportRowDto>();
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@tenant", SqlDbType.NVarChar, 64){ Value = tenantId });
        cmd.Parameters.Add(new SqlParameter("@term", SqlDbType.NVarChar, 64){ Value = (object?)termId ?? DBNull.Value });
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        while (await rdr.ReadAsync(ct))
        {
            list.Add(new EnrollmentReportRowDto(
                rdr.GetString(0),
                rdr.GetString(1),
                (int)rdr.GetInt32(2),   //TODO
                (int)rdr.GetInt64(3)
            ));
        }
        return list;
    }

    public async Task<IReadOnlyList<RevenueReportRowDto>> GetRevenueReportAsync(string tenantId, string? termId, CancellationToken ct = default)
    {
        const string sql = @"
SELECT i.TermId, SUM(il.Amount) AS Total
FROM Invoices i WITH (NOLOCK)
JOIN InvoiceLines il WITH (NOLOCK) ON il.InvoiceId = i.Id
WHERE i.TenantId = @tenant AND i.Status = 'Paid' AND (@term IS NULL OR i.TermId = @term)
GROUP BY i.TermId
ORDER BY i.TermId;";
        var list = new List<RevenueReportRowDto>();
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@tenant", SqlDbType.NVarChar, 64){ Value = tenantId });
        cmd.Parameters.Add(new SqlParameter("@term", SqlDbType.NVarChar, 64){ Value = (object?)termId ?? DBNull.Value });
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        while (await rdr.ReadAsync(ct))
        {
            list.Add(new RevenueReportRowDto(
                rdr.GetString(0),
                new MoneyDto(rdr.GetDecimal(1), "PHP"),
                new MoneyDto(rdr.GetDecimal(1)) //TODO:
            ));
        }
        return list;
    }

    public async Task<IReadOnlyList<InstructorLoadRowDto>> GetInstructorLoadReportAsync(string tenantId, string? termId, CancellationToken ct = default)
    {
        const string sql = @"
SELECT s.InstructorId, COUNT_BIG(1) AS Sections, SUM(c.Units) AS Units
FROM Sections s WITH (NOLOCK)
JOIN Courses c WITH (NOLOCK) ON c.Id = s.CourseId AND c.TenantId = s.TenantId
WHERE s.TenantId = @tenant AND (@term IS NULL OR s.TermId = @term)
GROUP BY s.InstructorId
ORDER BY s.InstructorId;";
        var list = new List<InstructorLoadRowDto>();
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@tenant", SqlDbType.NVarChar, 64){ Value = tenantId });
        cmd.Parameters.Add(new SqlParameter("@term", SqlDbType.NVarChar, 64){ Value = (object?)termId ?? DBNull.Value });
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        while (await rdr.ReadAsync(ct))
        {
            list.Add(new InstructorLoadRowDto(
                rdr.IsDBNull(0) ? "" : rdr.GetString(0),
                (int)rdr.GetInt64(1),
                rdr.IsDBNull(2) ? 0 : rdr.GetInt32(2)
            ));
        }
        return list;
    }

    public async Task<IReadOnlyList<RetentionCohortRowDto>> GetRetentionCohortReportAsync(string tenantId, int? cohortYear, CancellationToken ct = default)
    {
        // Simple placeholder computation: row per cohort year with 0% retained to keep compile-friendly structure.
        await Task.Yield();
        var year = cohortYear ?? System.DateTimeOffset.UtcNow.Year;
        return new List<RetentionCohortRowDto> { new RetentionCohortRowDto($"{year}", 0, 0, 0) };
    }
}
