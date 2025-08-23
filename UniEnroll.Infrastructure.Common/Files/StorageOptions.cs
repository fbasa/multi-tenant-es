namespace UniEnroll.Infrastructure.Common.Files;

public sealed class StorageOptions
{
    /// <summary>Root path for local file storage. Default: /tmp/unienroll-files</summary>
    public string RootPath { get; set; } = "/tmp/unienroll-files";
}
