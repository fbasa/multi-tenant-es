
using FluentValidation;

namespace UniEnroll.Application.Features.StudentPortal.Queries.GetStudentDashboard;

public sealed class GetStudentDashboardQueryValidator : AbstractValidator<GetStudentDashboardQuery>
{
    public GetStudentDashboardQueryValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.StudentId).NotEmpty();
        RuleFor(x => x.TermId).NotEmpty();
    }
}
