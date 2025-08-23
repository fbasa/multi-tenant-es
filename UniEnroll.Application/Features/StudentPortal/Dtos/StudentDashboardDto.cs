
using UniEnroll.Application.Features.StudentPortal.Dtos;

namespace UniEnroll.Application.Features.StudentPortal.Dtos;

public sealed record StudentDashboardDto(string StudentId, string TermId, int UnitsEnrolled, int HoldsCount, UniEnroll.Contracts.Common.MoneyDto Balance, string[] Alerts);
