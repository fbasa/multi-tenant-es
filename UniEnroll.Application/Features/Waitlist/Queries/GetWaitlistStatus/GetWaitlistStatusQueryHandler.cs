
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Waitlist.Dtos;

namespace UniEnroll.Application.Features.Waitlist.Queries.GetWaitlistStatus;

public sealed class GetWaitlistStatusQueryHandler : IRequestHandler<GetWaitlistStatusQuery, Result<WaitlistStatusDto>>
{
    public Task<Result<WaitlistStatusDto>> Handle(GetWaitlistStatusQuery request, CancellationToken ct)
        => Task.FromResult(Result<WaitlistStatusDto>.Success(new WaitlistStatusDto(request.SectionId, request.StudentId, 0, false)));
}
