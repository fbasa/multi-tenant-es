
using System.Text.RegularExpressions;

namespace UniEnroll.Domain.Students.ValueObjects;

public readonly struct Email
{
    public string Value { get; }
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new System.ArgumentException("Invalid email", nameof(value));
        Value = value;
    }
    public override string ToString() => Value;
}
