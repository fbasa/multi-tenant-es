
namespace UniEnroll.Contracts.Instructors;

public sealed record AssignInstructorToSectionRequest(string InstructorId, Guid SectionId);
