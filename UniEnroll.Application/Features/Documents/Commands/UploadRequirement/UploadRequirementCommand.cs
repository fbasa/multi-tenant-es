
using System.IO;
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Infrastructure.Common.Abstractions;

namespace UniEnroll.Application.Features.Documents.Commands;

public sealed record UploadRequirementCommand(string TenantId, string StudentId, string FileName, byte[] Content) : IRequest<Result<string>>;

public sealed class UploadRequirementHandler : IRequestHandler<UploadRequirementCommand, Result<string>>
{
    private readonly IFileStorage _files;
    public UploadRequirementHandler(IFileStorage files) => _files = files;

    public async Task<Result<string>> Handle(UploadRequirementCommand request, CancellationToken ct)
    {
        using var ms = new MemoryStream(request.Content);
        var path = $"requirements/{request.StudentId}/{request.FileName}";
        await _files.SaveAsync(ms, path, ct);
        return Result<string>.Success(path);
    }
}
