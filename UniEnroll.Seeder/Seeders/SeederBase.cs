
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UniEnroll.Seeder.Seeders;

internal abstract class SeederBase : UniEnroll.Seeder.IDataSeeder
{
    protected readonly ILogger _logger;
    protected readonly string _cs;

    protected SeederBase(ILogger logger, IConfiguration config)
    {
        _logger = logger;
        _cs = config.GetConnectionString("Sql") ?? config["Sql:ConnectionString"] ?? string.Empty;
    }

    public abstract Task SeedAsync(CancellationToken ct);

    protected async Task<int> ExecAsync(string sql, IEnumerable<SqlParameter> parameters, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        foreach (var p in parameters) cmd.Parameters.Add(p);
        return await cmd.ExecuteNonQueryAsync(ct);
    }

    protected static SqlParameter P(string name, object? value, SqlDbType type, int? size = null)
    {
        var p = new SqlParameter(name, type) { Value = value ?? DBNull.Value };
        if (size.HasValue) p.Size = size.Value;
        return p;
    }
}
