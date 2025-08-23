
using FluentValidation;

namespace UniEnroll.Application.Features.Registrar.Commands.UpsertTerm;

public sealed class UpsertTermCommandValidator : AbstractValidator<UpsertTermCommand>
{
    public UpsertTermCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.YearTermCode).NotEmpty().MaximumLength(16);
        RuleFor(x => x.Status).NotEmpty();
    }
}
