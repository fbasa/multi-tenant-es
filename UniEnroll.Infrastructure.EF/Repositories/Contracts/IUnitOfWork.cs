
using System.Threading;
using System.Threading.Tasks;

namespace UniEnroll.Infrastructure.EF.Repositories.Contracts;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
