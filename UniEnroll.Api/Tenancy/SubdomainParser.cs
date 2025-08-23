
namespace UniEnroll.Api.Tenancy;

public static class SubdomainParser
{
    /// <summary>Extracts tenant from host like 'tenant.app.university.edu.ph' -> 'tenant'.</summary>
    public static string? GetTenantFromHost(string host)
    {
        if (string.IsNullOrWhiteSpace(host)) return null;
        var parts = host.Split('.');
        return parts.Length >= 3 ? parts[0] : null;
    }
}
