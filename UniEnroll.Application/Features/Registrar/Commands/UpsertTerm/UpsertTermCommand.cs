
using MediatR;
using System;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Registrar.Commands.Common;
using UniEnroll.Contracts.Registrar;

namespace UniEnroll.Application.Features.Registrar.Commands.UpsertTerm;

public sealed record UpsertTermCommand(
    UpsertTermRequest Request
) : IRequest<Result<UpsertTermResult>>;
