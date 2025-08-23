
using FluentValidation;

namespace UniEnroll.Application.Features.Tenants.Commands.UpsertTenant;

public sealed class UpsertTenantCommandValidator : AbstractValidator<UpsertTenantCommand>
{
    public UpsertTenantCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
        RuleFor(x => x.PartitionKey).NotEmpty().MaximumLength(64);
    }
}
