
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Payments.Commands.RefundPayment;

public sealed record RefundPaymentCommand(string TenantId, string PaymentId) : IRequest<Result<bool>>;

public sealed class RefundPaymentHandler : IRequestHandler<RefundPaymentCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(RefundPaymentCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true));
}
