
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniEnroll.Api.Auth;

namespace UniEnroll.Api.Controllers;

public sealed class WaitlistController : BaseApiController
{
    public WaitlistController(MediatR.ISender sender) : base(sender) { }

    [HttpGet("{tenantId}/sections/{sectionId}")]
    public IActionResult Get([FromRoute] string tenantId, [FromRoute] string sectionId)
        => Ok(new { sectionId, waitlistCount = 0 });

    //TODO:
    [HttpPost("{tenantId}/join")]
    [Authorize(Policy = Policies.Student.Enroll)]
    public Task<IActionResult> Join(/*…*/) => null;

    [HttpPost("{tenantId}/leave")]
    [Authorize(Policy = Policies.Student.Enroll)]
    public Task<IActionResult> Leave(/*…*/) => null;

    [HttpGet("{tenantId}/status")]
    [Authorize(Policy = Policies.Student.Enroll)]
    public Task<IActionResult> Status(/*…*/) => null;
}
