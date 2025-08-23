
namespace UniEnroll.Application.Features.Registrar.Dtos;

public sealed record GraduationAuditDto(string StudentId, bool Eligible, string[] MissingCourseIds);
