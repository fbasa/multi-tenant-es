namespace UniEnroll.Contracts.Registrar;

public sealed record GraduationAuditDto(
    string StudentId,
    bool Eligible,
    string[] MissingCourseIds);
