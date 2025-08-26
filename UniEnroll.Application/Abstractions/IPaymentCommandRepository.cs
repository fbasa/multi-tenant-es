using UniEnroll.Contracts.Payments;

namespace UniEnroll.Application.Abstractions;

public interface IPaymentCommandRepository
{
    Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest request, CancellationToken ct);
    Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest request, CancellationToken ct);
}
