
namespace UniEnroll.Application.Features.Registrar.Commands.Common;

public enum RegistrarOutcome
{
    Inserted,
    Updated,
    Conflict,
    NotFound,
    ValidationFailed
}

public sealed record UpsertTermResult(RegistrarOutcome Outcome);
public sealed record SetEnrollmentWindowResult(RegistrarOutcome Outcome);
