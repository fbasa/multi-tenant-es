namespace UniEnroll.Contracts.CourseSearch;

/// <summary>Keyset-friendly search request; Next/Prev carry opaque cursors.</summary>
public sealed record CourseSearchRequest(
    string TenantId,
    string? Keyword,
    string? Department,
    string? InstructorId,
    string? DayFilter,
    string? TimeFrom,
    string? TimeTo,
    int PageSize = 50,
    string? Next = null,
    string? Prev = null);
