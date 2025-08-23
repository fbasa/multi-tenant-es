
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Enrollment.Commands.ReserveSeat;

public sealed record ReserveSeatCommand(string TenantId, string SectionId, string StudentId) : IRequest<Result<string>>;

public sealed class ReserveSeatHandler : IRequestHandler<ReserveSeatCommand, Result<string>>
{
    public Task<Result<string>> Handle(ReserveSeatCommand request, CancellationToken ct)
        => Task.FromResult(Result<string>.Success($"reservation-{request.SectionId}-{request.StudentId}"));
}
