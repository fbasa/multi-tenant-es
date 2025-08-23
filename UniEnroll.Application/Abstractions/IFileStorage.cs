
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace UniEnroll.Application.Abstractions;

public interface IFileStorage
{
    Task<string> SaveAsync(Stream content, string path, CancellationToken ct = default);
    Task<Stream> OpenReadAsync(string path, CancellationToken ct = default);
}
