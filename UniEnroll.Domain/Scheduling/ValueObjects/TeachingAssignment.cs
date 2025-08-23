
namespace UniEnroll.Domain.Scheduling.ValueObjects;

public readonly struct TeachingAssignment
{
    public string InstructorId { get; }
    public TeachingAssignment(string instructorId) => InstructorId = instructorId;
}
