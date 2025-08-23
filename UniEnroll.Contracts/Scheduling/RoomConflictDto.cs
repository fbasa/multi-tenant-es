namespace UniEnroll.Contracts.Scheduling;

public sealed record RoomConflictDto(
    string SectionAId,
    string SectionBId,
    string Room,
    string TermId);
