using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Api.Auth;
using UniEnroll.Application.Features.Billing.Commands.GenerateInvoice;
using UniEnroll.Application.Features.Billing.Queries.GetLedger;
using UniEnroll.Contracts.Billing;

namespace UniEnroll.Api.Controllers;

public sealed class BillingController : BaseApiController
{
    public BillingController(ISender sender) : base(sender) { }

    [HttpPost("{tenantId}/invoices/generate")]
    [Authorize(Policy = Policies.Registrar.ManageBilling)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Generate([FromRoute] string tenantId, [FromBody] GenerateInvoiceCommand body, CancellationToken ct)
        => Ok((await Sender.Send(body with { TenantId = tenantId }, ct)).Value);

    [HttpGet("{tenantId}/students/{studentId}/ledger")]
    [Authorize(Policy = Policies.Student.ViewLedger)]
    [ProducesResponseType(typeof(IReadOnlyList<InvoiceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Ledger([FromRoute] string tenantId, [FromRoute] string studentId, CancellationToken ct)
        => Ok((await Sender.Send(new GetLedgerQuery(tenantId, studentId), ct)).Value);
}
