
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Admissions.Queries;

public sealed record GetApplicationStatusQuery(string TenantId, string ApplicationId) : IRequest<Result<string>>;

public sealed class GetApplicationStatusHandler : IRequestHandler<GetApplicationStatusQuery, Result<string>>
{
    private readonly IRepositoryBase<Domain.Admissions.Application> _q;
    public GetApplicationStatusHandler(IRepositoryBase<Domain.Admissions.Application> q) => _q = q;

    public async Task<Result<string>> Handle(GetApplicationStatusQuery request, CancellationToken ct)
    {
        var app = await _q.GetAsync(a => a.Id == request.ApplicationId && a.TenantId == request.TenantId, ct);
        return app is null ? Result<string>.Failure("Not found") : Result<string>.Success(app.Status);
    }
}
