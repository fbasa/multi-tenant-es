namespace UniEnroll.Contracts.Students;

public sealed record RegisterStudentRequest(string UserId, string FirstName, string LastName, string Email, string ProgramId, int EntryYear);

