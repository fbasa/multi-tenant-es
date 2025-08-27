using UniEnroll.Contracts.Grades;

namespace UniEnroll.Infrastructure.EF.Repositories.Contracts;

public interface IGradeRepository
{
    Task<RecordGradeResult> RecordGradeAsync(RecordGradeRequest request);
}
