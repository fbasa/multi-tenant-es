
using System.Collections.Generic;

namespace UniEnroll.Domain.Registrar;

public sealed class AcademicCalendar
{
    public string TermId { get; }
    public List<(string Name, System.DateTimeOffset Date)> Milestones { get; } = new();
    public AcademicCalendar(string termId) { TermId = termId; }
    public void Add(string name, System.DateTimeOffset date) => Milestones.Add((name, date));
}
