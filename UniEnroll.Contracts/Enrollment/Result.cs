namespace UniEnroll.Contracts.Enrollment;

public enum EnrollmentOutcome
{
    Enrolled,
    Waitlisted,
    NoSeats,
    AlreadyEnrolled,
    WindowClosed,
    Conflict,
    NotFound,
    ValidationFailed
}

public sealed record ReserveSeatResult(
    EnrollmentOutcome Outcome,
    Guid? ReservationId,
    DateTimeOffset? ExpiresAt);

public sealed record EnrollSeatResult(
    EnrollmentOutcome Outcome,
    string? EnrollmentId);

public sealed record DropResult(
    EnrollmentOutcome Outcome,
    bool PromotedFromWaitlist);
