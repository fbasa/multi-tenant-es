
namespace UniEnroll.Domain.Scheduling.ValueObjects;

public readonly struct RoomAssignment
{
    public string RoomCode { get; }
    public RoomAssignment(string roomCode) => RoomCode = roomCode;
}
