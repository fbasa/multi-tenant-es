
namespace UniEnroll.Domain.Abstractions;

public abstract class AggregateRoot : Entity
{
    protected AggregateRoot(string id) : base(id) { }
}
