using UniEnroll.Contracts.Common;

namespace UniEnroll.Contracts.Billing;

public sealed record InvoiceDto(
    string Id,
    string StudentId,
    MoneyDto Amount,
    string Status,
    string TermId);
