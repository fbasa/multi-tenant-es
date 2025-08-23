
namespace UniEnroll.Infrastructure.EF.Query.Projections.Reports;

public sealed record EnrollmentAggregate(string TermId, string CourseId, string SectionId, int Count);
