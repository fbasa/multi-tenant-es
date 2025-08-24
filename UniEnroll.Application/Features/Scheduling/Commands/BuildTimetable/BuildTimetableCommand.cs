
using System;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Scheduling.Commands.Common;

namespace UniEnroll.Application.Features.Scheduling.Commands.BuildTimetable;

public sealed record BuildTimetableCommand(string StudentId, Guid TermId) : IRequest<Result<BuildTimetableResult>>;
