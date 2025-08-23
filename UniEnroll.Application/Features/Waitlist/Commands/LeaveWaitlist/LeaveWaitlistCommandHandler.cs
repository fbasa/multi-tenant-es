
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Waitlist.Commands.LeaveWaitlist;

public sealed class LeaveWaitlistCommandHandler : IRequestHandler<LeaveWaitlistCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(LeaveWaitlistCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true));
}
