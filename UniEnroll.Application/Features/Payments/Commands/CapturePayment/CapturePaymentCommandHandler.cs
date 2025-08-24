
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Payments.Commands.Common;

namespace UniEnroll.Application.Features.Payments.Commands.CapturePayment;

public sealed class CapturePaymentCommandHandler : IRequestHandler<CapturePaymentCommand, Result<CapturePaymentResult>>
{
    private readonly IPaymentCommandRepository _repo;
    public CapturePaymentCommandHandler(IPaymentCommandRepository repo) => _repo = repo;

    public async Task<Result<CapturePaymentResult>> Handle(CapturePaymentCommand r, CancellationToken ct)
        => Result<CapturePaymentResult>.Success(await _repo.CaptureAsync(r.Request, ct));
}
