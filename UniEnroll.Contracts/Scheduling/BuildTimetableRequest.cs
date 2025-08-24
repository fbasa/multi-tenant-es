namespace UniEnroll.Contracts.Scheduling;

public sealed record BuildTimetableRequest(string StudentId, Guid TermId);
