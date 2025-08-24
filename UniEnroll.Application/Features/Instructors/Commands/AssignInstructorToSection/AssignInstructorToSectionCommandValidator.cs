
using FluentValidation;

namespace UniEnroll.Application.Features.Instructors.Commands.AssignInstructorToSection;

public sealed class AssignInstructorToSectionCommandValidator : AbstractValidator<AssignInstructorToSectionCommand>
{
    public AssignInstructorToSectionCommandValidator()
    {
        RuleFor(x => x.SectionId).NotEmpty();
        RuleFor(x => x.InstructorId).NotEmpty().MaximumLength(64);
    }
}
