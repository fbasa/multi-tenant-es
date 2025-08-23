
using System;

namespace UniEnroll.Infrastructure.EF.Query.Views;

public sealed record CourseSearchView(string CourseId, string SectionId, string Code, string Title, int Units, string InstructorId, DayOfWeek[] Days, TimeSpan Start, TimeSpan End, string? Room);
