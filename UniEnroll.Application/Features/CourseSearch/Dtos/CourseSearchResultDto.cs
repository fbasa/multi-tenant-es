
using System;

namespace UniEnroll.Application.Features.CourseSearch.Dtos;

public sealed record CourseSearchResultDto(string CourseId, string SectionId, string Code, string Title, int Units, string InstructorId, DayOfWeek[] Days, TimeSpan Start, TimeSpan End, string? Room);
