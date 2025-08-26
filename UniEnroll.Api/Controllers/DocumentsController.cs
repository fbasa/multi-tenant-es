using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Api.Auth;
using UniEnroll.Application.Features.Documents.Commands;
using UniEnroll.Application.Features.Documents.Queries;
using UniEnroll.Contracts.Documents;

namespace UniEnroll.Api.Controllers;

public sealed class DocumentsController : BaseApiController
{
    public DocumentsController(ISender sender) : base(sender) { }

    [HttpPost("{tenantId}/students/{studentId}/requirements")]
    [Authorize(Policy = Policies.Student.Read)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Upload([FromRoute] string tenantId, [FromRoute] string studentId, [FromBody] UploadRequirementCommand body, CancellationToken ct)
        => Ok((await Sender.Send(body with { TenantId = tenantId, StudentId = studentId }, ct)).Value);

    [HttpGet("{tenantId}/students/{studentId}/requirements")]
    [Authorize(Policy = Policies.Student.UploadRequirements)]
    [ProducesResponseType(typeof(IReadOnlyList<RequirementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromRoute] string tenantId, [FromRoute] string studentId, CancellationToken ct)
        => Ok((await Sender.Send(new ListRequirementsQuery(tenantId, studentId), ct)).Value);
}
