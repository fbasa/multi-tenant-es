
using System;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Contracts.Payments;

namespace UniEnroll.Application.Abstractions;

public interface IPaymentQueryRepository
{
    Task<PaymentStatusResult?> GetStatusAsync(Guid paymentId, CancellationToken ct);
}
