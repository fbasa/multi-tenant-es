
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Scheduling.Queries.Common;

namespace UniEnroll.Application.Features.Scheduling.Queries.ListRoomConflicts;

public sealed class ListRoomConflictsQueryHandler : IRequestHandler<ListRoomConflictsQuery, Result<IReadOnlyList<RoomConflictDto>>>
{
    private readonly ISchedulingRepository _repo;
    public ListRoomConflictsQueryHandler(ISchedulingRepository repo) => _repo = repo;

    public async Task<Result<IReadOnlyList<RoomConflictDto>>> Handle(ListRoomConflictsQuery request, CancellationToken ct)
        => Result<IReadOnlyList<RoomConflictDto>>.Success(await _repo.ListRoomConflictsAsync(request.TermId, ct));
}
