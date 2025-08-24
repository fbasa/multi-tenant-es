
using System;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Application.Features.Payments.Commands.Common;
using UniEnroll.Application.Features.Payments.Commands.RefundPayment;
using UniEnroll.Contracts.Payments;

namespace UniEnroll.Application.Abstractions;

public interface IPaymentCommandRepository
{
    Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest request, CancellationToken ct);
    Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest request, CancellationToken ct);
}
