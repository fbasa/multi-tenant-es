
using System;

namespace UniEnroll.Application.Common;

public static class Guard
{
    public static void AgainstNull(object? value, string name)
    {
        if (value is null) throw new ArgumentNullException(name);
    }

    public static void AgainstNullOrWhiteSpace(string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException($"{name} is required", name);
    }

    public static void AgainstOutOfRange(bool condition, string message)
    {
        if (condition) throw new ArgumentOutOfRangeException(message);
    }
}
