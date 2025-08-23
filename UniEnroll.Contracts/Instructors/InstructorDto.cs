namespace UniEnroll.Contracts.Instructors;

public sealed record InstructorDto(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string Department);
