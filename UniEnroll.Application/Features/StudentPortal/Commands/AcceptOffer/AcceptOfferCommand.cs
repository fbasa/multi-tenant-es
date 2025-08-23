
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.StudentPortal.Commands.AcceptOffer;

public sealed record AcceptOfferCommand(string TenantId, string StudentId) : IRequest<Result<bool>>;

public sealed class AcceptOfferHandler : IRequestHandler<AcceptOfferCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(AcceptOfferCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true));
}
