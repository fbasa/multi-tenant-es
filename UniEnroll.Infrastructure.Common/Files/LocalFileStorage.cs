using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using UniEnroll.Infrastructure.Common.Abstractions;

namespace UniEnroll.Infrastructure.Common.Files;

public sealed class LocalFileStorage : IFileStorage
{
    private readonly string _root;
    public LocalFileStorage(IOptions<StorageOptions> opts)
    {
        _root = string.IsNullOrWhiteSpace(opts.Value.RootPath) ? "/tmp/unienroll-files" : opts.Value.RootPath;
        Directory.CreateDirectory(_root);
    }

    public async Task<string> SaveAsync(Stream content, string path, CancellationToken ct = default)
    {
        var full = Path.Combine(_root, path.Replace("..", string.Empty).TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(Path.GetDirectoryName(full)!);
        using var fs = File.Create(full);
        await content.CopyToAsync(fs, ct);
        return full;
    }

    public Task<Stream> OpenReadAsync(string path, CancellationToken ct = default)
    {
        var full = Path.Combine(_root, path.Replace("..", string.Empty).TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        Stream s = File.OpenRead(full);
        return Task.FromResult(s);
    }
}
