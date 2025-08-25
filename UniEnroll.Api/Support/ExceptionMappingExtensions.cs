using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UniEnroll.Infrastructure.Common.Tenancy;

namespace UniEnroll.Api.Support;

public static class ExceptionMappingExtensions
{
    public static void MapProblemDetailsExceptions(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async ctx =>
            {
                ctx.Response.ContentType = "application/problem+json";

                var ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
                var problem = ex switch
                {
                    ValidationException v => new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation failed",
                        Detail = string.Join("; ", v.Errors.Select(e => e.ErrorMessage))
                    },
                    DbUpdateConcurrencyException => new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Concurrency conflict",
                        Detail = "The resource was modified by another request. Please retry."
                    },
                    _ => new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Internal Server Error",
                        Detail = "An unexpected error occurred."
                    }
                };

                problem.Extensions["correlationId"] = ctx.Items.TryGetValue(TenantHeaderNames.CorrelationId, out var corr) ? corr : null;
                ctx.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
                await ctx.Response.WriteAsJsonAsync(problem);
            });
        });
    }
}
