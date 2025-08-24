
using System;
using System.Collections.Generic;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Scheduling.Queries.Common;

namespace UniEnroll.Application.Features.Scheduling.Queries.ListRoomConflicts;

public sealed record ListRoomConflictsQuery(Guid TermId) : IRequest<Result<IReadOnlyList<RoomConflictDto>>>;
