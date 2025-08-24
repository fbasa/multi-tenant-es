
namespace UniEnroll.Contracts.Instructors;

public sealed record UpsertInstructorRequest(string FirstName, string LastName, string Email, string Department, string InstructorId);
