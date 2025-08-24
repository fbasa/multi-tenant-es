
using System;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Application.Features.Payments.Queries.Common;

namespace UniEnroll.Application.Abstractions;

public interface IPaymentQueryRepository
{
    Task<PaymentStatusResult?> GetStatusAsync(Guid paymentId, CancellationToken ct);
}
