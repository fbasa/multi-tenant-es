
using System;

namespace UniEnroll.Application.Common;

public readonly struct Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    private Result(bool ok, T? value, string? error)
    {
        IsSuccess = ok; Value = value; Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);

    public void Deconstruct(out bool ok, out T? value, out string? error)
    { ok = IsSuccess; value = Value; error = Error; }
}
