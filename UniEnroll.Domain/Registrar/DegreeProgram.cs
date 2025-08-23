
namespace UniEnroll.Domain.Registrar;

public sealed class DegreeProgram
{
    public string Id { get; }
    public string Name { get; }
    public DegreeProgram(string id, string name) { Id = id; Name = name; }
}
