
namespace UniEnroll.Domain.Sections.ValueObjects;

public readonly struct Room
{
    public string Code { get; }
    public Room(string code) => Code = code;
    public override string ToString() => Code;
}
