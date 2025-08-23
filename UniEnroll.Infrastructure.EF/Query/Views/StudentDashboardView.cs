
namespace UniEnroll.Infrastructure.EF.Query.Views;

public sealed record StudentDashboardView(string StudentId, string TermId, int UnitsEnrolled, int HoldsCount, decimal Balance);
