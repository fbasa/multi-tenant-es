
using System;

namespace UniEnroll.Domain.Scheduling;

public sealed class ScheduleSlot
{
    public string SectionId { get; }
    public ValueObjects.DayOfWeekVO Day { get; }
    public ValueObjects.TimeRange Time { get; }
    public string? RoomCode { get; }

    public ScheduleSlot(string sectionId, ValueObjects.DayOfWeekVO day, ValueObjects.TimeRange time, string? roomCode)
    { SectionId = sectionId; Day = day; Time = time; RoomCode = roomCode; }
}
