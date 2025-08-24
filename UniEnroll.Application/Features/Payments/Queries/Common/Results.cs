
namespace UniEnroll.Application.Features.Payments.Queries.Common;

public sealed record PaymentStatusResult(string PaymentId, string Status, decimal Amount, string Currency);
