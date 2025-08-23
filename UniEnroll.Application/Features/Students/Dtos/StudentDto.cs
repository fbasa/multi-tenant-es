
namespace UniEnroll.Application.Features.Students.Dtos;

public sealed record StudentDto(string Id, string UserId, string FirstName, string LastName, string Email, string ProgramId, int EntryYear);
