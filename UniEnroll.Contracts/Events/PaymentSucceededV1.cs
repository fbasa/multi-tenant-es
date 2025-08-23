
using System;
using UniEnroll.Contracts.Common;

namespace UniEnroll.Contracts.Events;

public sealed record PaymentSucceededV1(string PaymentId, string InvoiceId, MoneyDto Amount, DateTimeOffset OccurredAt);
