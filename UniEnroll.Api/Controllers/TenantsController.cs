
using Microsoft.AspNetCore.Mvc;

namespace UniEnroll.Api.Controllers;

public sealed class TenantsController : BaseApiController
{
    public TenantsController(MediatR.ISender sender) : base(sender) { }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult List() => Ok(new[] { new { Id = "default", Name = "Default" } });
}
