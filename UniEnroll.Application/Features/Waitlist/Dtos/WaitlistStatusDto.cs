
namespace UniEnroll.Application.Features.Waitlist.Dtos;

public sealed record WaitlistStatusDto(string SectionId, string StudentId, int Position, bool IsPromoted);
