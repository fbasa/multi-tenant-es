
using MediatR;
using UniEnroll.Application.Common;
using UniEnroll.Contracts.Grades;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Application.Features.Grades.Commands;

public sealed record RecordGradeCommand(RecordGradeRequest Request) : IRequest<Result<RecordGradeResult>>;


public sealed class RecordGradeHandler : IRequestHandler<RecordGradeCommand, Result<RecordGradeResult>>
{
    private readonly IGradeRepository _repo;
    public RecordGradeHandler(IGradeRepository repo) => _repo = repo;

    public async Task<Result<RecordGradeResult>> Handle(RecordGradeCommand r, CancellationToken ct)
    {
        var res = await _repo.RecordGradeAsync(r.Request);
        return Result<RecordGradeResult>.Success(res);
    }
}