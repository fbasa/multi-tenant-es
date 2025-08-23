namespace UniEnroll.Contracts.Reporting;

public sealed record InstructorLoadReportRowDto(
    string InstructorId,
    int Units,
    int Sections);
