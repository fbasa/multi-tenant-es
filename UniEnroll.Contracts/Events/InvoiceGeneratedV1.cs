
using System;
using UniEnroll.Contracts.Common;

namespace UniEnroll.Contracts.Events;

public sealed record InvoiceGeneratedV1(string InvoiceId, MoneyDto Amount, DateTimeOffset OccurredAt);
