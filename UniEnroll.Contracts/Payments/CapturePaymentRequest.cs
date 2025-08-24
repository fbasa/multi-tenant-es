
using UniEnroll.Contracts.Common;

namespace UniEnroll.Contracts.Payments;

public sealed record CapturePaymentRequest(
        Guid InvoiceId,
        decimal Amount,
        string Currency,
        string Method,
        string? GatewayTxnId,
        string? IdempotencyKey
    );
