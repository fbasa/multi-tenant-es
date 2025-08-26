using UniEnroll.Contracts.Common;

namespace UniEnroll.Contracts.StudentPortal;

public sealed record StudentDashboardDto(string StudentId, string TermId, int UnitsEnrolled, int HoldsCount, MoneyDto Balance, string[] Alerts);

