
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Instructors;

namespace UniEnroll.Application.Features.Instructors.Queries.GetInstructorById;

public sealed record GetInstructorByIdQuery(string TenantId, string InstructorId) : IRequest<Result<InstructorDto>>;

public sealed class GetInstructorByIdHandler : IRequestHandler<GetInstructorByIdQuery, Result<InstructorDto>>
{
    public Task<Result<InstructorDto>> Handle(GetInstructorByIdQuery request, CancellationToken ct)
    {
        var dto = new InstructorDto(request.InstructorId, "First", "Last", "email@example.com", "Dept");
        return Task.FromResult(Result<InstructorDto>.Success(dto));
    }
}
