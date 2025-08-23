namespace UniEnroll.Contracts.Waitlist;

public sealed record WaitlistStatusDto(
    string SectionId,
    int Position,
    int Total);
