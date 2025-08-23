
namespace UniEnroll.Domain.Identity;

public sealed class Permission
{
    public string Id { get; }
    public string Name { get; }
    public Permission(string id, string name) { Id = id; Name = name; }
}
