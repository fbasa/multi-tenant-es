
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Features.Instructors.Commands.Common;
using UniEnroll.Infrastructure.EF.Sql;

namespace UniEnroll.Infrastructure.EF.Repositories;

public sealed class InstructorCommandRepository : IInstructorCommandRepository
{
    private readonly string _cs;
    public InstructorCommandRepository(IConfiguration config)
        => _cs = config.GetConnectionString("Sql") ?? config["Sql:ConnectionString"] ?? string.Empty;

    public async Task<UpsertInstructorResult> UpsertInstructorAsync(string instructorId, string firstName, string lastName, string email, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(InstructorSql.Upsert, conn);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, 64){ Value = instructorId });
        cmd.Parameters.Add(new SqlParameter("@first", SqlDbType.NVarChar, 64){ Value = firstName });
        cmd.Parameters.Add(new SqlParameter("@last", SqlDbType.NVarChar, 64){ Value = lastName });
        cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 256){ Value = email });
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        if (!await rdr.ReadAsync(ct)) return new UpsertInstructorResult(InstructorOutcome.Conflict, instructorId);
        var outcome = rdr.GetString(0);
        return outcome switch
        {
            "Inserted" => new UpsertInstructorResult(InstructorOutcome.Inserted, instructorId),
            "Updated"  => new UpsertInstructorResult(InstructorOutcome.Updated, instructorId),
            _          => new UpsertInstructorResult(InstructorOutcome.Conflict, instructorId)
        };
    }

    public async Task<AssignInstructorResult> AssignInstructorToSectionAsync(Guid sectionId, string instructorId, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(InstructorSql.AssignToSection, conn);
        cmd.Parameters.Add(new SqlParameter("@section", SqlDbType.UniqueIdentifier){ Value = sectionId });
        cmd.Parameters.Add(new SqlParameter("@instructor", SqlDbType.NVarChar, 64){ Value = instructorId });
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        if (!await rdr.ReadAsync(ct)) return new AssignInstructorResult(InstructorOutcome.Conflict);
        var outcome = rdr.GetString(0);
        return outcome switch
        {
            "Assigned"         => new AssignInstructorResult(InstructorOutcome.Assigned),
            "ValidationFailed" => new AssignInstructorResult(InstructorOutcome.ValidationFailed),
            _                  => new AssignInstructorResult(InstructorOutcome.Conflict)
        };
    }
}
