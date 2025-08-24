
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Application.Features.Scheduling.Commands.Common;
using UniEnroll.Application.Features.Scheduling.Queries.Common;

namespace UniEnroll.Application.Abstractions;

public interface ISchedulingRepository
{
    // Commands
    Task<BuildTimetableResult> BuildTimetableAsync(string studentId, Guid termId, CancellationToken ct);
    Task<AssignRoomResult> AssignRoomAsync(Guid sectionId, string roomCode, CancellationToken ct);
    Task<OptimizeScheduleResult> OptimizeAsync(Guid? termId, CancellationToken ct);

    // Queries
    Task<IReadOnlyList<ScheduleEntryDto>> GetStudentScheduleAsync(string studentId, Guid termId, CancellationToken ct);
    Task<IReadOnlyList<RoomConflictDto>> ListRoomConflictsAsync(Guid termId, CancellationToken ct);
}
