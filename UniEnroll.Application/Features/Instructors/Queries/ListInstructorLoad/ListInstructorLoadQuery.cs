
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Instructors;

namespace UniEnroll.Application.Features.Instructors.Queries.ListInstructorLoad;

public sealed record ListInstructorLoadQuery(string TenantId, string InstructorId) : IRequest<Result<InstructorLoadDto>>;

public sealed class ListInstructorLoadHandler : IRequestHandler<ListInstructorLoadQuery, Result<InstructorLoadDto>>
{
    public Task<Result<InstructorLoadDto>> Handle(ListInstructorLoadQuery request, CancellationToken ct)
        => Task.FromResult(Result<InstructorLoadDto>.Success(new InstructorLoadDto(request.InstructorId, 0, 0)));
}
