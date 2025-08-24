
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Payments.Commands.Common;

namespace UniEnroll.Application.Features.Payments.Commands.RefundPayment;

public sealed class RefundPaymentCommandHandler(IPaymentCommandRepository repo) : IRequestHandler<RefundPaymentCommand, Result<RefundPaymentResult>>
{
    public async Task<Result<RefundPaymentResult>> Handle(RefundPaymentCommand r, CancellationToken ct)
        => Result<RefundPaymentResult>.Success(await repo.RefundAsync(r.Request, ct));
}
