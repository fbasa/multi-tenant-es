
using System.Reflection;
using MediatR;
using UniEnroll.Application.Abstractions;

namespace UniEnroll.Application.Common.Behaviors;

/// <summary>
/// Copies TenantId from request (if present) into ITenantContext for downstream services and EF filters.
/// </summary>
public sealed class TenantPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ITenantContext _tenant;
    public TenantPipelineBehavior(ITenantContext tenant) => _tenant = tenant;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var prop = request!.GetType().GetProperty("TenantId", BindingFlags.Public | BindingFlags.Instance);
        if (prop is not null)
        {
            var value = prop.GetValue(request) as string;
            if (!string.IsNullOrWhiteSpace(value)) _tenant.TenantId = value;
        }
        return await next();
    }
}
