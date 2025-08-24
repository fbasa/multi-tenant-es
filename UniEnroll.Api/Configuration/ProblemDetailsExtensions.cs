
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UniEnroll.Api.Configuration;

public static class ProblemDetailsExtensions
{
    // Registers options + sensible defaults
    public static IServiceCollection AddProblemDetailsExtensions(this IServiceCollection services)
    {
        services.AddProblemDetails();

        services.AddOptions<ProblemDetailsMappingOptions>()
            .PostConfigure(o =>
            {
                o.TypeToStatus[typeof(FluentValidation.ValidationException)] = StatusCodes.Status400BadRequest;
                // Defaults (override with MapProblemDetailsExceptions below)
                o.TypeToStatus[typeof(ArgumentException)] = StatusCodes.Status400BadRequest;
                o.TypeToStatus[typeof(UnauthorizedAccessException)] = StatusCodes.Status401Unauthorized;
                o.TypeToStatus[typeof(KeyNotFoundException)] = StatusCodes.Status404NotFound;
                o.TypeToStatus[typeof(InvalidOperationException)] = StatusCodes.Status409Conflict; // generic “conflict”
#if NET8_0_OR_GREATER
                o.TypeToStatus[typeof(Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)] = StatusCodes.Status409Conflict;
#endif
            });

        return services;
    }
}

// Simple options bag the middleware can read
public sealed class ProblemDetailsMappingOptions
{
    public Dictionary<Type, int> TypeToStatus { get; } = new();
    public bool IncludeExceptionDetails { get; set; } = false;
}
