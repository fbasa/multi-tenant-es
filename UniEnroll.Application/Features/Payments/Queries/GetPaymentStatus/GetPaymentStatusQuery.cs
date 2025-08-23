
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Payments;
using UniEnroll.Contracts.Common;

namespace UniEnroll.Application.Features.Payments.Queries.GetPaymentStatus;

public sealed record GetPaymentStatusQuery(string TenantId, string PaymentId) : IRequest<Result<PaymentDto>>;

public sealed class GetPaymentStatusHandler : IRequestHandler<GetPaymentStatusQuery, Result<PaymentDto>>
{
    public Task<Result<PaymentDto>> Handle(GetPaymentStatusQuery request, CancellationToken ct)
    {
        var dto = new PaymentDto(request.PaymentId, "", new MoneyDto(0m), "Captured", DateTimeOffset.UtcNow);
        return Task.FromResult(Result<PaymentDto>.Success(dto));
    }
}
