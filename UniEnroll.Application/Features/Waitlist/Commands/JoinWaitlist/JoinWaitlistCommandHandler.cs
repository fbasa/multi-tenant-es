
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Waitlist.Commands.JoinWaitlist;

public sealed class JoinWaitlistCommandHandler : IRequestHandler<JoinWaitlistCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(JoinWaitlistCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true));
}
