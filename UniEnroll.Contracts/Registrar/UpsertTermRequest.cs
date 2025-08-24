
namespace UniEnroll.Contracts.Registrar;

public sealed record UpsertTermRequest(
    Guid TermId,
    string Code,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    byte[]? RowVersion
    );
