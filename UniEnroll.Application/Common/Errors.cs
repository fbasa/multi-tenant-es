
namespace UniEnroll.Application.Common;

public static class Errors
{
    public static string NotFound(string entity, string id) => $"{entity} '{id}' not found";
    public static string ConcurrencyConflict => "The resource was modified by another request. Please retry.";
    public static string ValidationFailed => "Validation failed.";
}
