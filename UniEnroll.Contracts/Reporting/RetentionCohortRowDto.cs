namespace UniEnroll.Contracts.Reporting;

public sealed record RetentionCohortRowDto(
    string CohortYear,
    int Size,
    int RetainedYear2,
    int RetainedYear3);
