
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UniEnroll.Messaging.Abstractions;

public interface IEventConsumer
{
    /// <summary>Topic routing keys this consumer wants to receive.</summary>
    IReadOnlyCollection<string> Topics { get; }

    /// <summary>Handle raw event bytes for the given routing key.</summary>
    Task HandleAsync(ReadOnlyMemory<byte> body, string routingKey, IDictionary<string, object?> headers, CancellationToken ct);
}
