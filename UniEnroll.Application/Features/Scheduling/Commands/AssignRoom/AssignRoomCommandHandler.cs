
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Scheduling.Commands.Common;

namespace UniEnroll.Application.Features.Scheduling.Commands.AssignRoom;

public sealed class AssignRoomCommandHandler : IRequestHandler<AssignRoomCommand, Result<AssignRoomResult>>
{
    private readonly ISchedulingRepository _repo;
    public AssignRoomCommandHandler(ISchedulingRepository repo) => _repo = repo;

    public async Task<Result<AssignRoomResult>> Handle(AssignRoomCommand request, CancellationToken ct)
        => Result<AssignRoomResult>.Success(await _repo.AssignRoomAsync(request.SectionId, request.RoomCode, ct));
}
