
using MediatR;
using System;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Payments;

namespace UniEnroll.Application.Features.Payments.Queries;

public sealed record GetPaymentStatusQuery(Guid PaymentId) : IRequest<Result<PaymentStatusResult?>>;

public sealed class GetPaymentStatusQueryHandler : IRequestHandler<GetPaymentStatusQuery, Result<PaymentStatusResult?>>
{
    private readonly IPaymentQueryRepository _repo;
    public GetPaymentStatusQueryHandler(IPaymentQueryRepository repo) => _repo = repo;

    public async Task<Result<PaymentStatusResult?>> Handle(GetPaymentStatusQuery request, CancellationToken ct)
        => Result<PaymentStatusResult>.Success(await _repo.GetStatusAsync(request.PaymentId, ct));
}

