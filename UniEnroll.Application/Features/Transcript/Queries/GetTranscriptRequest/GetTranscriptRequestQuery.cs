
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Transcript;

namespace UniEnroll.Application.Features.Transcript.Queries;

public sealed record GetTranscriptRequestQuery(string TenantId, string RequestId) : IRequest<Result<TranscriptRequestDto>>;

public sealed class GetTranscriptRequestHandler : IRequestHandler<GetTranscriptRequestQuery, Result<TranscriptRequestDto>>
{
    public Task<Result<TranscriptRequestDto>> Handle(GetTranscriptRequestQuery request, CancellationToken ct)
        => Task.FromResult(Result<TranscriptRequestDto>.Success(new TranscriptRequestDto(request.RequestId, "", "Pending", DateTimeOffset.UtcNow)));
}
