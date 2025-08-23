namespace UniEnroll.Contracts.StudentPortal;

public sealed record PlanMyTermSuggestionDto(
    string CourseId,
    string SectionId,
    bool MeetsPrereqs,
    bool NoConflicts);
