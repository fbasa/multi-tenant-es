
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Registrar.Commands;

public sealed record PublishCourseCatalogCommand(string TenantId) : IRequest<Result<bool>>;

public sealed class PublishCourseCatalogHandler : IRequestHandler<PublishCourseCatalogCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(PublishCourseCatalogCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true));
}
