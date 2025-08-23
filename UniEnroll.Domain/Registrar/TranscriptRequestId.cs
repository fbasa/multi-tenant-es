
namespace UniEnroll.Domain.Registrar;

public readonly struct TranscriptRequestId
{
    public string Value { get; }
    public TranscriptRequestId(string value) => Value = value;
    public override string ToString() => Value;
}
