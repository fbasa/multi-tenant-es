
namespace UniEnroll.Application.Features.Instructors.Commands.Common;

public enum InstructorOutcome
{
    Inserted,
    Updated,
    Assigned,
    Conflict,
    NotFound,
    ValidationFailed
}

public sealed record UpsertInstructorResult(InstructorOutcome Outcome, string InstructorId);
public sealed record AssignInstructorResult(InstructorOutcome Outcome);
