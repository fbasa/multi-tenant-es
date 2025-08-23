
using System.Collections.Generic;

namespace UniEnroll.Domain.Scheduling;

public sealed class Timetable
{
    public string StudentId { get; }
    public string TermId { get; }
    public List<ScheduleSlot> Slots { get; } = new();

    public Timetable(string studentId, string termId) { StudentId = studentId; TermId = termId; }
    public void Add(ScheduleSlot slot) => Slots.Add(slot);
}
