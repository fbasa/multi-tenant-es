
using Microsoft.AspNetCore.Mvc;

namespace UniEnroll.Api.Controllers;

public sealed class WaitlistController : BaseApiController
{
    public WaitlistController(MediatR.ISender sender) : base(sender) { }

    [HttpGet("{tenantId}/sections/{sectionId}")]
    public IActionResult Get([FromRoute] string tenantId, [FromRoute] string sectionId)
        => Ok(new { sectionId, waitlistCount = 0 });
}
