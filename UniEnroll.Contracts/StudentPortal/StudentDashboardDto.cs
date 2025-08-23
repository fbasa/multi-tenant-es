using UniEnroll.Contracts.Common;

namespace UniEnroll.Contracts.StudentPortal;

public sealed record StudentDashboardDto(
    string StudentId,
    string TermId,
    int OpenTodoCount,
    int ActiveHolds,
    MoneyDto OutstandingBalance,
    string[] CurrentSectionIds);
