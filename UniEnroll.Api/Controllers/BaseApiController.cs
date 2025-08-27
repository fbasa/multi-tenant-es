using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UniEnroll.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected readonly ISender Sender;
    protected BaseApiController(ISender sender) => Sender = sender;
}
