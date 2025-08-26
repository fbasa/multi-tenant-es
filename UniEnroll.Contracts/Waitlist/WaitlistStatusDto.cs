namespace UniEnroll.Contracts.Waitlist;

public sealed record WaitlistStatusDto(string SectionId, string StudentId, int Position, bool IsPromoted);
