
namespace UniEnroll.Application.Features.CourseSearch.Filters;

public sealed record CourseSearchFilters(string? Keyword, string? Department, string? InstructorId, string? DayFilter, string? TimeFrom, string? TimeTo);
