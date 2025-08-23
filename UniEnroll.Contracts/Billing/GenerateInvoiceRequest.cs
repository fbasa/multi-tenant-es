
using System.Collections.Generic;

namespace UniEnroll.Contracts.Billing;

public sealed record GenerateInvoiceRequest(string StudentId, string TermId, IReadOnlyList<InvoiceLineRequest> Lines);

public sealed record InvoiceLineRequest(string Description, decimal Amount);
