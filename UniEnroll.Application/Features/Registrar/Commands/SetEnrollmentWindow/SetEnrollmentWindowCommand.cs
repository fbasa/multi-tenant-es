
using System;
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Registrar.Commands.SetEnrollmentWindow;

public sealed record SetEnrollmentWindowCommand(string TenantId, DateTimeOffset OpensAt, DateTimeOffset ClosesAt) : IRequest<Result<bool>>;

public sealed class SetEnrollmentWindowHandler : IRequestHandler<SetEnrollmentWindowCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(SetEnrollmentWindowCommand request, CancellationToken ct)
        => Task.FromResult(Result<bool>.Success(true));
}
