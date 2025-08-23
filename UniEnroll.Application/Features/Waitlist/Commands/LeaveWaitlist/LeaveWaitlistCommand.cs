
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Waitlist.Commands.LeaveWaitlist;

public sealed record LeaveWaitlistCommand(string TenantId, string SectionId, string StudentId) : IRequest<Result<bool>>;
