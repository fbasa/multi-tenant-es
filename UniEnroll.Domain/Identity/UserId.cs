
namespace UniEnroll.Domain.Identity;

public readonly struct UserId
{
    public string Value { get; }
    public UserId(string value) => Value = value;
    public override string ToString() => Value;
}
