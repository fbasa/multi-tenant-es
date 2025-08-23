using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Application.Features.Admissions.Commands.Apply;
using UniEnroll.Application.Features.Admissions.Queries.GetApplicationStatus;

namespace UniEnroll.Api.Controllers;

public sealed class AdmissionsController : BaseApiController
{
    public AdmissionsController(ISender sender) : base(sender) { }

    [AllowAnonymous]
    [HttpPost("apply")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Apply([FromBody] ApplyCommand cmd, CancellationToken ct)
        => Ok((await Sender.Send(cmd, ct)).Value);

    [HttpGet("{tenantId}/applications/{applicationId}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatus([FromRoute] string tenantId, [FromRoute] string applicationId, CancellationToken ct)
        => Ok((await Sender.Send(new GetApplicationStatusQuery(tenantId, applicationId), ct)).Value);
}
