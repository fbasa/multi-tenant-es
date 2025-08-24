
using System;
using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Application.Features.Instructors.Commands.Common;

namespace UniEnroll.Application.Abstractions;

public interface IInstructorCommandRepository
{
    Task<UpsertInstructorResult> UpsertInstructorAsync(string instructorId, string firstName, string lastName, string email, CancellationToken ct);
    Task<AssignInstructorResult> AssignInstructorToSectionAsync(Guid sectionId, string instructorId, CancellationToken ct);
}
