
using FluentValidation;

namespace UniEnroll.Application.Features.Scheduling.Commands.BuildTimetable;

public sealed class BuildTimetableCommandValidator : AbstractValidator<BuildTimetableCommand>
{
    public BuildTimetableCommandValidator()
    {
        RuleFor(x => x.StudentId).NotEmpty().MaximumLength(64);
        RuleFor(x => x.TermId).NotEmpty();
    }
}
