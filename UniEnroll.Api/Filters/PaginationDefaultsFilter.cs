
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UniEnroll.Api.Filters;

/// <summary>Caps pageSize and ensures sane defaults.</summary>
public sealed class PaginationDefaultsFilter : IActionFilter
{
    private const int MaxPageSize = 200;
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var qs = context.HttpContext.Request.Query;
        if (qs.TryGetValue("size", out var sizeStr) && int.TryParse(sizeStr, out var size) && size > MaxPageSize)
        {
            var pd = new ProblemDetails { Title = "pageSize too large", Status = StatusCodes.Status400BadRequest, Detail = $"Max pageSize is {MaxPageSize}." };
            context.Result = new ObjectResult(pd) { StatusCode = StatusCodes.Status400BadRequest };
        }
    }
    public void OnActionExecuted(ActionExecutedContext context) { }
}
