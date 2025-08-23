
namespace UniEnroll.Domain.Registrar;

public sealed class StudentHold
{
    public string Id { get; }
    public string StudentId { get; }
    public string HoldType { get; }
    public string Status { get; private set; }

    public StudentHold(string id, string studentId, string type, string status)
    { Id = id; StudentId = studentId; HoldType = type; Status = status; }

    public void Clear() => Status = "Cleared";
}
