
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Students.Commands;

public sealed record UpdateNotificationPrefsCommand(string TenantId, string StudentId, bool EmailEnabled, bool SmsEnabled, bool PushEnabled)
    : IRequest<Result<bool>>;

public sealed class UpdateNotificationPrefsHandler : IRequestHandler<UpdateNotificationPrefsCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(UpdateNotificationPrefsCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true)); // persist where appropriate
}
