
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Abstractions;
using UniEnroll.Domain.Sections;
using UniEnroll.Domain.Sections.ValueObjects;

namespace UniEnroll.Application.Features.Scheduling.Commands.AssignRoom;

public sealed record AssignRoomCommand(string TenantId, string SectionId, string Room) : IRequest<Result<bool>>;

public sealed class AssignRoomHandler : IRequestHandler<AssignRoomCommand, Result<bool>>
{
    private readonly IRepository<Section> _repo;
    private readonly IUnitOfWork _uow;
    public AssignRoomHandler(IRepository<Section> repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<Result<bool>> Handle(AssignRoomCommand request, CancellationToken ct)
    {
        var sec = await _repo.GetAsync(s => s.Id == request.SectionId, ct);
        if (sec is null) return Result<bool>.Failure("Not found");
        sec.AssignRoom(new Room(request.Room));
        await _uow.SaveChangesAsync(ct);
        return Result<bool>.Success(true);
    }
}
