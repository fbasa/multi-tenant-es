
namespace UniEnroll.Domain.Abstractions;

/// <summary>Represents a SQL rowversion/etag.</summary>
public readonly struct ConcurrencyToken
{
    public byte[] Value { get; }
    public ConcurrencyToken(byte[] value) { Value = value ?? System.Array.Empty<byte>(); }
}
