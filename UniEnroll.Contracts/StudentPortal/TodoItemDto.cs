namespace UniEnroll.Contracts.StudentPortal;

public sealed record TodoItemDto(
    string Id,
    string Type,
    string Label,
    string ActionUrl);
