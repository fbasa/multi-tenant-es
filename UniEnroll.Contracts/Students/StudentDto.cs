namespace UniEnroll.Contracts.Students;

public sealed record StudentDto(string Id, string UserId, string FirstName, string LastName, string Email, string ProgramId, int EntryYear);

