
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;
using UniEnroll.Contracts.Scheduling;
using UniEnroll.Domain.Sections;

namespace UniEnroll.Application.Features.Scheduling.Queries.ListRoomConflicts;

public sealed record ListRoomConflictsQuery(string TenantId, string TermId) : IRequest<Result<IReadOnlyList<RoomConflictDto>>>;

public sealed class ListRoomConflictsHandler : IRequestHandler<ListRoomConflictsQuery, Result<IReadOnlyList<RoomConflictDto>>>
{
    private readonly IQueryRepository<Section> _q;
    public ListRoomConflictsHandler(IQueryRepository<Section> q) => _q = q;

    public async Task<Result<IReadOnlyList<RoomConflictDto>>> Handle(ListRoomConflictsQuery request, CancellationToken ct)
    {
        var sections = await _q.ListAsync(s => s.TermId == request.TermId, ct);
        var conflicts = new List<RoomConflictDto>();
        foreach (var a in sections.Where(s => s.Room != null))
        foreach (var b in sections.Where(s => s.Room != null))
        {
            if (a.Id == b.Id) continue;
            if (a.Room!.Value.Code == b.Room!.Value.Code && a.StartTime < b.EndTime && b.StartTime < a.EndTime)
                conflicts.Add(new RoomConflictDto(a.Id, b.Id, a.Room.Value.Code, request.TermId));
        }
        return Result<IReadOnlyList<RoomConflictDto>>.Success(conflicts);
    }
}
