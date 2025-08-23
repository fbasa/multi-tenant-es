using System;
using UniEnroll.Contracts.Common;

namespace UniEnroll.Contracts.Payments;

public sealed record PaymentDto(
    string PaymentId,
    string InvoiceId,
    MoneyDto Amount,
    string Status,
    DateTimeOffset CreatedAt);
