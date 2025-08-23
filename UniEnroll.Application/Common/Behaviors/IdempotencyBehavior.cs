
using MediatR;

namespace UniEnroll.Application.Common.Behaviors;

/// <summary>
/// Optional no-op idempotency guard. Real implementation can hash request and consult a store.
/// Included to satisfy pipeline composition without infrastructure dependency.
/// </summary>
public sealed class IdempotencyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        => await next();
}
