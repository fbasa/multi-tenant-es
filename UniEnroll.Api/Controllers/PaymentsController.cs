
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Api.Auth;
using UniEnroll.Application.Features.Payments.Commands.Common;
using UniEnroll.Application.Features.Payments.Commands.CapturePayment;
using UniEnroll.Application.Features.Payments.Commands.RefundPayment;
using UniEnroll.Application.Features.Payments.Queries.GetPaymentStatus;
using UniEnroll.Contracts.Payments;
using Asp.Versioning;

namespace UniEnroll.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/payments")]
public sealed class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public PaymentsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("capture")]
    [Authorize(Policy = Policies.Billing.Capture)]
    public async Task<IActionResult> Capture([FromBody] CapturePaymentRequest req, CancellationToken ct)
    {
        var r = await _mediator.Send(new CapturePaymentCommand(req), ct);
        return r.Value.Outcome switch
        {
            PaymentOutcome.Captured        => Ok(new { paymentId = r.Value.PaymentId }),
            PaymentOutcome.AlreadyCaptured => Ok(new { paymentId = r.Value.PaymentId }),
            PaymentOutcome.NotFound        => NotFound(),
            _                              => StatusCode(409, new ProblemDetails { Title = "Payment conflict" })
        };
    }

    [HttpPost("{paymentId:guid}/refund")]
    [Authorize(Policy = Policies.Billing.Refund)]
    public async Task<IActionResult> Refund([FromRoute] Guid paymentId, [FromBody] RefundPaymentRequest req, CancellationToken ct)
    {
        if (paymentId != req.PaymentId) return BadRequest(new ProblemDetails { Title = "PaymentId mismatch" });
        var r = await _mediator.Send(new RefundPaymentCommand(req), ct);
        return r.Value?.Outcome switch
        {
            PaymentOutcome.Refunded => Ok(new { refundPaymentId = r.Value.RefundPaymentId }),
            PaymentOutcome.NotFound => NotFound(),
            _                       => StatusCode(409, new ProblemDetails { Title = "Refund conflict" })
        };
    }

    [HttpGet("{paymentId:guid}")]
    [Authorize(Policy = Policies.Billing.View)]
    public async Task<IActionResult> GetStatus([FromRoute] Guid paymentId, CancellationToken ct)
    {
        var r = await _mediator.Send(new GetPaymentStatusQuery(paymentId), ct);
        return r.Value is null ? NotFound() : Ok(r.Value);
    }
}
