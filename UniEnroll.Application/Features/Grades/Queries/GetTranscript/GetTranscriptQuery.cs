
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Grades;

namespace UniEnroll.Application.Features.Grades.Queries.GetTranscript;

public sealed record GetTranscriptQuery(string TenantId, string StudentId) : IRequest<Result<TranscriptDto>>;

public sealed class GetTranscriptHandler : IRequestHandler<GetTranscriptQuery, Result<TranscriptDto>>
{
    public Task<Result<TranscriptDto>> Handle(GetTranscriptQuery request, CancellationToken ct)
    {
        var dto = new TranscriptDto(request.StudentId, Array.Empty<TranscriptLineDto>(), 0m);
        return Task.FromResult(Result<TranscriptDto>.Success(dto));
    }
}
