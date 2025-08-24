
using FluentValidation;
using System;

namespace UniEnroll.Application.Features.Registrar.Commands.UpsertTerm;

public sealed class UpsertTermCommandValidator : AbstractValidator<UpsertTermCommand>
{
    public UpsertTermCommandValidator()
    {
        //RuleFor(x => x.TermId).NotEmpty();
        //RuleFor(x => x.Code).NotEmpty().MaximumLength(32);
        //RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
        //RuleFor(x => x.StartDate).LessThan(x => x.EndDate);
    }
}
