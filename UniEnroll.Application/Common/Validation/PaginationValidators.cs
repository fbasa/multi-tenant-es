
using FluentValidation;

namespace UniEnroll.Application.Common.Validation;

public sealed class OffsetPaginationValidator : AbstractValidator<(int page, int size)>
{
    public OffsetPaginationValidator()
    {
        RuleFor(x => x.page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.size).InclusiveBetween(1, 200);
    }
}

public sealed class KeysetPaginationValidator : AbstractValidator<(int pageSize, string? next, string? prev)>
{
    public KeysetPaginationValidator()
    {
        RuleFor(x => x.pageSize).InclusiveBetween(1, 200);
        RuleFor(x => x).Must(x => string.IsNullOrWhiteSpace(x.next) || string.IsNullOrWhiteSpace(x.prev))
            .WithMessage("Only one of 'next' or 'prev' may be provided.");
    }
}
