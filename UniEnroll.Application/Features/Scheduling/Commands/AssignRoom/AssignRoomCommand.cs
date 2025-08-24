
using System;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Application.Features.Scheduling.Commands.Common;

namespace UniEnroll.Application.Features.Scheduling.Commands.AssignRoom;

public sealed record AssignRoomCommand(Guid SectionId, string RoomCode) : IRequest<Result<AssignRoomResult>>;
