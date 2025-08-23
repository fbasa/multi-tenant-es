using UniEnroll.Contracts.Common;

namespace UniEnroll.Contracts.Reporting;

public sealed record RevenueReportRowDto(
    string TermId,
    MoneyDto TotalInvoiced,
    MoneyDto TotalPaid);
