using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace UniEnroll.Application.Common.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidationBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);
        var failures = (await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, ct))))
                       .SelectMany(r => r.Errors).Where(f => f is not null).ToList();
        if (failures.Count != 0)
        {
            logger.LogWarning("Validation failed for {RequestType}", typeof(TRequest).Name);
            throw new ValidationException(failures);
        }
        return await next();
    }
}
