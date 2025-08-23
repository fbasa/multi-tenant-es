namespace UniEnroll.Contracts.Documents;

public sealed record RequirementDto(
    string Id,
    string StudentId,
    string Type,
    string Status,
    string? ReviewerNotes);
