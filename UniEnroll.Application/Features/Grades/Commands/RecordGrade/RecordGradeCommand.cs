
using MediatR;
using UniEnroll.Application.Common;

namespace UniEnroll.Application.Features.Grades.Commands;

public enum GradeOutcome
{
    Inserted,
    ValidationFailed
}
public sealed record RecordGradeResult(GradeOutcome Outcome, int? Id);

public interface IGradeRepository
{
    Task<RecordGradeResult> RecordGradeAsync(RecordGradeRequest request);
}
public sealed class GradeRepository : IGradeRepository
{
    public async Task<RecordGradeResult> RecordGradeAsync(RecordGradeRequest request)
    {
        //Dummany result from db
        var (result,id) = await Task.FromResult(("Inserted",1));

        return result switch
        {
            "Inserted" => new RecordGradeResult(GradeOutcome.Inserted, id),
            _ => new RecordGradeResult(GradeOutcome.ValidationFailed, null)
        };
    }
}

public sealed record RecordGradeRequest(string TenantId, string EnrollmentId, string Grade);

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