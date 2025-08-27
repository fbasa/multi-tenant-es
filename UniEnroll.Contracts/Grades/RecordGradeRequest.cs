namespace UniEnroll.Contracts.Grades;

public sealed record RecordGradeRequest(string TenantId, string EnrollmentId, string Grade);