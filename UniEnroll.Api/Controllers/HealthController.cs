using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UniEnroll.Api.Controllers;

[AllowAnonymous]
public sealed class HealthController : BaseApiController
{
    public HealthController(ISender sender) : base(sender) { }

    [HttpGet("ping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Ping() => Ok(new { status = "ok" });
}
