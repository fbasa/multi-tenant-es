
namespace UniEnroll.Domain.Abstractions;

public interface IRowVersioned
{
    byte[] RowVersion { get; }
}
