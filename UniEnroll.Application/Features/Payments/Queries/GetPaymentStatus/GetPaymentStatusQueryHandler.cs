
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Payments.Queries.Common;

namespace UniEnroll.Application.Features.Payments.Queries.GetPaymentStatus;

public sealed class GetPaymentStatusQueryHandler : IRequestHandler<GetPaymentStatusQuery, Result<PaymentStatusResult?>>
{
    private readonly IPaymentQueryRepository _repo;
    public GetPaymentStatusQueryHandler(IPaymentQueryRepository repo) => _repo = repo;

    public async Task<Result<PaymentStatusResult?>> Handle(GetPaymentStatusQuery request, CancellationToken ct)
        => Result<PaymentStatusResult>.Success(await _repo.GetStatusAsync(request.PaymentId, ct));
}
