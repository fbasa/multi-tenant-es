
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UniEnroll.Application.Abstractions;
using UniEnroll.Contracts.Scheduling;
using UniEnroll.Infrastructure.EF.Sql;

namespace UniEnroll.Infrastructure.EF.Repositories;

public sealed class SchedulingRepository : ISchedulingRepository
{
    private readonly string _cs;
    public SchedulingRepository(IConfiguration cfg)
        => _cs = cfg.GetConnectionString("Sql") ?? cfg["Sql:ConnectionString"] ?? string.Empty;

    public async Task<BuildTimetableResult> BuildTimetableAsync(string studentId, Guid termId, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(SchedulingSql.BuildTimetable, conn);
        cmd.Parameters.Add(new SqlParameter("@student", SqlDbType.NVarChar, 64){ Value = studentId });
        cmd.Parameters.Add(new SqlParameter("@term", SqlDbType.UniqueIdentifier){ Value = termId });
        var created = 0;
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        if (await rdr.ReadAsync(ct)) created = rdr.GetInt32(0);
        return new BuildTimetableResult(SchedulingOutcome.Success, created);
    }

    public async Task<AssignRoomResult> AssignRoomAsync(Guid sectionId, string roomCode, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(SchedulingSql.AssignRoomWithClashCheck, conn);
        cmd.Parameters.Add(new SqlParameter("@section", SqlDbType.UniqueIdentifier){ Value = sectionId });
        cmd.Parameters.Add(new SqlParameter("@room", SqlDbType.NVarChar, 32){ Value = roomCode });
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        if (!await rdr.ReadAsync(ct)) return new AssignRoomResult(SchedulingOutcome.Conflict);
        var outcome = rdr.GetString(0);
        return outcome == "Success" ? new AssignRoomResult(SchedulingOutcome.Success) : new AssignRoomResult(SchedulingOutcome.Conflict);
    }

    public async Task<OptimizeScheduleResult> OptimizeAsync(Guid? termId, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        var conflicts = 0;
        if (termId is not null)
        {
            await using var cmd = new SqlCommand(SchedulingSql.DetectRoomConflicts, conn);
            cmd.Parameters.Add(new SqlParameter("@term", SqlDbType.UniqueIdentifier){ Value = termId });
            await using var rdr = await cmd.ExecuteReaderAsync(ct);
            if (await rdr.ReadAsync(ct)) conflicts = rdr.GetInt32(0);
        }
        return new OptimizeScheduleResult(SchedulingOutcome.Success, conflicts);
    }

    public async Task<IReadOnlyList<ScheduleEntryDto>> GetStudentScheduleAsync(string studentId, Guid termId, CancellationToken ct)
    {
        var list = new List<ScheduleEntryDto>();
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(SchedulingSql.GetStudentSchedule, conn);
        cmd.Parameters.Add(new SqlParameter("@student", SqlDbType.NVarChar, 64){ Value = studentId });
        cmd.Parameters.Add(new SqlParameter("@term", SqlDbType.UniqueIdentifier){ Value = termId });
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        while (await rdr.ReadAsync(ct))
        {
            list.Add(new ScheduleEntryDto(
                SectionId: rdr.GetGuid(0),
                CourseCode: rdr.GetString(1),
                Room: rdr.GetString(2),
                DayOfWeek: rdr.GetInt32(3),
                StartTime: rdr.GetString(4),
                EndTime: rdr.GetString(5)
            ));
        }
        return list;
    }

    public async Task<IReadOnlyList<RoomConflictDto>> ListRoomConflictsAsync(Guid termId, CancellationToken ct)
    {
        var list = new List<RoomConflictDto>();
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(SchedulingSql.ListRoomConflicts, conn);
        cmd.Parameters.Add(new SqlParameter("@term", SqlDbType.UniqueIdentifier){ Value = termId });
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        while (await rdr.ReadAsync(ct))
        {
            list.Add(new RoomConflictDto(
                SectionId: rdr.GetGuid(0),
                Room: rdr.GetString(1),
                DayOfWeek: rdr.GetInt32(2),
                StartTime: rdr.GetString(3),
                EndTime: rdr.GetString(4),
                ConflictsWithSectionId: rdr.GetGuid(5)
            ));
        }
        return list;
    }
}
