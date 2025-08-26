
namespace UniEnroll.Contracts.Registrar;

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
