
using MediatR;
using System;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Registrar.Commands.Common;
using UniEnroll.Contracts.Registrar;

namespace UniEnroll.Application.Features.Registrar.Commands.SetEnrollmentWindow;

public sealed record SetEnrollmentWindowCommand(
    SetEnrollmentWindowRequest Request
) : IRequest<Result<SetEnrollmentWindowResult>>;
