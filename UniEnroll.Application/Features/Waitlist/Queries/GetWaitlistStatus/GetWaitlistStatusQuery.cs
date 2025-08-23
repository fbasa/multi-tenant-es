
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Waitlist.Dtos;

namespace UniEnroll.Application.Features.Waitlist.Queries.GetWaitlistStatus;

public sealed record GetWaitlistStatusQuery(string TenantId, string SectionId, string StudentId) : IRequest<Result<WaitlistStatusDto>>;
