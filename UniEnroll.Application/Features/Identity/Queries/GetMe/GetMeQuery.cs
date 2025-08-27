
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Infrastructure.Common.Abstractions;

namespace UniEnroll.Application.Features.Identity.Queries;

public sealed record GetMeQuery() : IRequest<Result<string>>;

public sealed class GetMeHandler : IRequestHandler<GetMeQuery, Result<string>>
{
    private readonly ICurrentUser _me;
    public GetMeHandler(ICurrentUser me) => _me = me;

    public Task<Result<string>> Handle(GetMeQuery request, CancellationToken ct)
        => Task.FromResult(Result<string>.Success(_me.Email ?? _me.UserId ?? "anonymous"));
}
