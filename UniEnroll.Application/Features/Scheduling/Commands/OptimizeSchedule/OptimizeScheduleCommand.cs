
using System;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Scheduling.Commands.Common;

namespace UniEnroll.Application.Features.Scheduling.Commands.OptimizeSchedule;

public sealed record OptimizeScheduleCommand(Guid? TermId) : IRequest<Result<OptimizeScheduleResult>>;
