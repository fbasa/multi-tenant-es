
using MediatR;
using System;
using System.Collections.Generic;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Scheduling;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Scheduling.Queries;

public sealed record ListRoomConflictsQuery(Guid TermId) : IRequest<Result<IReadOnlyList<RoomConflictDto>>>;

public sealed class ListRoomConflictsQueryHandler : IRequestHandler<ListRoomConflictsQuery, Result<IReadOnlyList<RoomConflictDto>>>
{
    private readonly ISchedulingRepository _repo;
    public ListRoomConflictsQueryHandler(ISchedulingRepository repo) => _repo = repo;

    public async Task<Result<IReadOnlyList<RoomConflictDto>>> Handle(ListRoomConflictsQuery request, CancellationToken ct)
        => Result<IReadOnlyList<RoomConflictDto>>.Success(await _repo.ListRoomConflictsAsync(request.TermId, ct));
}
