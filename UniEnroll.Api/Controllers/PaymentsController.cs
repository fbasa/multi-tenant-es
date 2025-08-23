using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Application.Features.Payments.Commands.CapturePayment;
using UniEnroll.Application.Features.Payments.Commands.RefundPayment;
using UniEnroll.Application.Features.Payments.Queries.GetPaymentStatus;
using UniEnroll.Contracts.Payments;

namespace UniEnroll.Api.Controllers;

public sealed class PaymentsController : BaseApiController
{
    public PaymentsController(ISender sender) : base(sender) { }

    [HttpPost("{tenantId}/capture")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Capture([FromRoute] string tenantId, [FromBody] CapturePaymentCommand body, CancellationToken ct)
        => Ok((await Sender.Send(body with { TenantId = tenantId }, ct)).Value);

    [HttpPost("{tenantId}/{paymentId}/refund")]
    public async Task<IActionResult> Refund([FromRoute] string tenantId, [FromRoute] string paymentId, CancellationToken ct)
        => Ok(await Sender.Send(new RefundPaymentCommand(tenantId, paymentId), ct));

    [HttpGet("{tenantId}/{paymentId}")]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Status([FromRoute] string tenantId, [FromRoute] string paymentId, CancellationToken ct)
        => Ok((await Sender.Send(new GetPaymentStatusQuery(tenantId, paymentId), ct)).Value);
}
