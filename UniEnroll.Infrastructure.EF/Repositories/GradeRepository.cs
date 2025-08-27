using UniEnroll.Contracts.Grades;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;

namespace UniEnroll.Infrastructure.EF.Repositories;
public sealed class GradeRepository : IGradeRepository
{
    public async Task<RecordGradeResult> RecordGradeAsync(RecordGradeRequest request)
    {
        //Dummany result from db
        var (result, id) = await Task.FromResult(("Inserted", 1));

        return result switch
        {
            "Inserted" => new RecordGradeResult(GradeOutcome.Inserted, id),
            _ => new RecordGradeResult(GradeOutcome.ValidationFailed, null)
        };
    }
}