
using System.Threading;
using System.Threading.Tasks;

namespace UniEnroll.Infrastructure.EF.Persistence.Outbox;

public sealed class OutboxProcessor
{
    public Task<int> ProcessPendingAsync(CancellationToken ct = default)
        => Task.FromResult(0);
}
