
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Waitlist.Commands.JoinWaitlist;

public sealed record JoinWaitlistCommand(string TenantId, string SectionId, string StudentId) : IRequest<Result<bool>>;
