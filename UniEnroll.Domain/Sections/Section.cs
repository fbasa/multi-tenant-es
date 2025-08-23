
using System;
using UniEnroll.Domain.Common;
using UniEnroll.Domain.Sections.ValueObjects;

namespace UniEnroll.Domain.Sections;

public sealed class Section : EntityBase, IAggregateRoot
{
    public string CourseId { get; private set; }
    public string TermId { get; private set; }
    public string InstructorId { get; private set; }
    public Capacity Capacity { get; private set; }
    public Room? Room { get; private set; }
    public DayOfWeek[] MeetingDays { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public int SeatsTaken { get; private set; }
    public string TenantId { get; private set; }
    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    public Section(string id, string courseId, string termId, string instructorId, Capacity capacity, DayOfWeek[] days, TimeSpan start, TimeSpan end, string tenantId) : base(id)
    {
        CourseId = courseId;
        TermId = termId;
        InstructorId = instructorId;
        Capacity = capacity;
        MeetingDays = days ?? Array.Empty<DayOfWeek>();
        StartTime = start;
        EndTime = end;
        TenantId = tenantId;
        SeatsTaken = 0;
    }

    public void AssignRoom(Room room) => Room = room;
    public void ReserveSeat() { if (SeatsTaken < Capacity.Total) SeatsTaken++; }
    public void ReleaseSeat() { if (SeatsTaken > 0) SeatsTaken--; }
}
