
using FluentValidation;

namespace UniEnroll.Application.Features.Enrollment.Commands.EnrollStudent;

public sealed class EnrollStudentCommandValidator : AbstractValidator<EnrollStudentCommand>
{
    public EnrollStudentCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.SectionId).NotEmpty();
        RuleFor(x => x.StudentId).NotEmpty();
    }
}
