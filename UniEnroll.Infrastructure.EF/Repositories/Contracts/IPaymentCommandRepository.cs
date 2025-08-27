using UniEnroll.Contracts.Payments;

namespace UniEnroll.Infrastructure.EF.Repositories.Contracts;

public interface IPaymentCommandRepository
{
    Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest request, CancellationToken ct);
    Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest request, CancellationToken ct);
}
