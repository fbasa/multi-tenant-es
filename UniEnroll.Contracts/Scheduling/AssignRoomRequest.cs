namespace UniEnroll.Contracts.Scheduling;

public sealed record AssignRoomRequest(Guid SectionId, string RoomCode);
