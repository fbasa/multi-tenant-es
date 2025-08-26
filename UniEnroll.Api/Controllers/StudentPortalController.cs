using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Api.Auth;
using UniEnroll.Application.Features.StudentPortal.Commands;
using UniEnroll.Application.Features.StudentPortal.Queries;
using UniEnroll.Contracts.StudentPortal;

namespace UniEnroll.Api.Controllers;

public sealed class StudentPortalController : BaseApiController
{
    public StudentPortalController(ISender sender) : base(sender) { }

    [HttpGet("{tenantId}/students/{studentId}/dashboard")]
    [Authorize(Policy = Policies.Student.Read)]
    [ProducesResponseType(typeof(StudentDashboardDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Dashboard([FromRoute] string tenantId, [FromRoute] string studentId, [FromQuery] string termId, CancellationToken ct)
        => Ok((await Sender.Send(new GetStudentDashboardQuery(tenantId, studentId, termId), ct)).Value);

    [HttpGet("{tenantId}/students/{studentId}/plan-term")]
    [ProducesResponseType(typeof(IReadOnlyList<PlanMyTermSuggestionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Plan([FromRoute] string tenantId, [FromRoute] string studentId, [FromQuery] string termId, CancellationToken ct)
        => Ok((await Sender.Send(new PlanMyTermQuery(tenantId, studentId, termId), ct)).Value);

    [HttpPost("{tenantId}/students/{studentId}/accept-offer")]
    [Authorize(Policy = Policies.Student.Enroll)]
    public async Task<IActionResult> AcceptOffer([FromRoute] string tenantId, [FromRoute] string studentId, [FromBody] AcceptOfferCommand body, CancellationToken ct)
        => Ok(await Sender.Send(body with { TenantId = tenantId, StudentId = studentId }, ct));
}
