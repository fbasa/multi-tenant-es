
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Payments.Commands.CapturePayment;

public sealed record CapturePaymentCommand(string TenantId, string InvoiceId, decimal Amount, string Currency) : IRequest<Result<string>>;

public sealed class CapturePaymentHandler : IRequestHandler<CapturePaymentCommand, Result<string>>
{
    public Task<Result<string>> Handle(CapturePaymentCommand request, CancellationToken ct)
        => Task.FromResult(Result<string>.Success($"pay-{Guid.NewGuid():N}"));
}
