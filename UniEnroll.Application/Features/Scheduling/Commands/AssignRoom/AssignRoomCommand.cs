
using FluentValidation;
using MediatR;
using System;
using UniEnroll.Application.Abstractions;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Scheduling;

namespace UniEnroll.Application.Features.Scheduling.Commands;

public sealed record AssignRoomCommand(Guid SectionId, string RoomCode) : IRequest<Result<AssignRoomResult>>;

public sealed class AssignRoomCommandValidator : AbstractValidator<AssignRoomCommand>
{
    public AssignRoomCommandValidator()
    {
        RuleFor(x => x.SectionId).NotEmpty();
        RuleFor(x => x.RoomCode).NotEmpty().MaximumLength(32);
    }
}

public sealed class AssignRoomCommandHandler : IRequestHandler<AssignRoomCommand, Result<AssignRoomResult>>
{
    private readonly ISchedulingRepository _repo;
    public AssignRoomCommandHandler(ISchedulingRepository repo) => _repo = repo;

    public async Task<Result<AssignRoomResult>> Handle(AssignRoomCommand request, CancellationToken ct)
        => Result<AssignRoomResult>.Success(await _repo.AssignRoomAsync(request.SectionId, request.RoomCode, ct));
}
