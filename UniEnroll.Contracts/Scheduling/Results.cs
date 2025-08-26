
namespace UniEnroll.Contracts.Scheduling;

public enum SchedulingOutcome
{
    Success,
    Conflict,
    ValidationFailed,
    NotFound
}

public sealed record BuildTimetableResult(SchedulingOutcome Outcome, int EntriesCreated);
public sealed record AssignRoomResult(SchedulingOutcome Outcome);
public sealed record OptimizeScheduleResult(SchedulingOutcome Outcome, int ConflictsRecorded);
