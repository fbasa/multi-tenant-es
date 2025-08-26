
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Billing.Commands;

public sealed record GenerateInvoiceCommand(string TenantId, string StudentId, decimal Amount, string Currency, string TermId) : IRequest<Result<string>>;

public sealed class GenerateInvoiceHandler : IRequestHandler<GenerateInvoiceCommand, Result<string>>
{
    public Task<Result<string>> Handle(GenerateInvoiceCommand request, CancellationToken ct)
        => Task.FromResult(Result<string>.Success($"inv-{Guid.NewGuid():N}"));
}
