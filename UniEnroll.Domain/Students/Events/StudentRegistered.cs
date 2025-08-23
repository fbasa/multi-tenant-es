
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Students.Events;

public sealed class StudentRegistered : DomainEvent
{
    public string StudentId { get; }
    public StudentRegistered(string studentId) { StudentId = studentId; }
}
