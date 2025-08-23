
using UniEnroll.Contracts.Common;

namespace UniEnroll.Contracts.Payments;

public sealed record CapturePaymentRequest(string InvoiceId, MoneyDto Amount, string Method);
