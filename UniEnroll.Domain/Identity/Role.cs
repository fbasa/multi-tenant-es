
namespace UniEnroll.Domain.Identity;

public sealed class Role
{
    public string Id { get; }
    public string Name { get; }
    public Role(string id, string name) { Id = id; Name = name; }
}
