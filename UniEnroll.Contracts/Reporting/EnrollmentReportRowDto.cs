namespace UniEnroll.Contracts.Reporting;

public sealed record EnrollmentReportRowDto(
    string TermId,
    string CourseId,
    int EnrolledCount,
    int WaitlistedCount)
{
    public decimal Count { get; set; }
}
