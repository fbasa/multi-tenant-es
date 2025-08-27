
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Contracts.Scheduling;

namespace UniEnroll.Infrastructure.EF.Repositories.Contracts;

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
