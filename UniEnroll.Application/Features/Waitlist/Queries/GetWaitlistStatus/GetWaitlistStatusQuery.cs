
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Waitlist;

namespace UniEnroll.Application.Features.Waitlist.Queries;

public sealed record GetWaitlistStatusQuery(string TenantId, string SectionId, string StudentId) : IRequest<Result<WaitlistStatusDto>>;

public sealed class GetWaitlistStatusQueryHandler : IRequestHandler<GetWaitlistStatusQuery, Result<WaitlistStatusDto>>
{
    public Task<Result<WaitlistStatusDto>> Handle(GetWaitlistStatusQuery request, CancellationToken ct)
        => Task.FromResult(Result<WaitlistStatusDto>.Success(new WaitlistStatusDto(request.SectionId, request.StudentId, 0, false)));
}

