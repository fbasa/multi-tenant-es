
using System;
using System.Collections.Generic;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Scheduling.Queries.Common;

namespace UniEnroll.Application.Features.Scheduling.Queries.GetStudentSchedule;

public sealed record GetStudentScheduleQuery(string StudentId, Guid TermId) : IRequest<Result<IReadOnlyList<ScheduleEntryDto>>>;
