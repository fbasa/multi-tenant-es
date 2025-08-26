
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Transcript.Commands;

public sealed record RequestTranscriptCommand(string TenantId, string StudentId) : IRequest<Result<string>>;

public sealed class RequestTranscriptHandler : IRequestHandler<RequestTranscriptCommand, Result<string>>
{
    public Task<Result<string>> Handle(RequestTranscriptCommand request, CancellationToken ct)
        => Task.FromResult(Result<string>.Success($"tr-{Guid.NewGuid():N}"));
}
