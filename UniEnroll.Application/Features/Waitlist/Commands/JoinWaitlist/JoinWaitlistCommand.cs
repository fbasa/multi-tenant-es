
using FluentValidation;
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Waitlist.Commands;

public sealed record JoinWaitlistCommand(string TenantId, string SectionId, string StudentId) : IRequest<Result<bool>>;

public sealed class JoinWaitlistCommandValidator : AbstractValidator<JoinWaitlistCommand>
{
    public JoinWaitlistCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.SectionId).NotEmpty();
        RuleFor(x => x.StudentId).NotEmpty();
    }
}

public sealed class JoinWaitlistCommandHandler : IRequestHandler<JoinWaitlistCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(JoinWaitlistCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true));
}
