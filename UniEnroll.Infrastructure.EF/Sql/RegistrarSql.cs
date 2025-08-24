
namespace UniEnroll.Infrastructure.EF.Sql;

public static class RegistrarSql
{
    public const string UpsertTerm = @"
SET XACT_ABORT ON;
BEGIN TRAN;

IF EXISTS (SELECT 1 FROM Terms WITH (UPDLOCK, HOLDLOCK) WHERE Id=@id)
BEGIN
    -- concurrency with rowversion if provided
    IF @rowver IS NOT NULL
    BEGIN
        UPDATE Terms SET Code=@code, Name=@name, StartDate=@start, EndDate=@end
        WHERE Id=@id AND RowVersion=@rowver;
        IF @@ROWCOUNT = 0 BEGIN SELECT 'Conflict' AS Outcome; ROLLBACK; RETURN; END
    END
    ELSE
    BEGIN
        UPDATE Terms SET Code=@code, Name=@name, StartDate=@start, EndDate=@end WHERE Id=@id;
    END
    SELECT 'Updated' AS Outcome;
END
ELSE
BEGIN
    INSERT INTO Terms (Id, Code, Name, StartDate, EndDate) VALUES (@id, @code, @name, @start, @end);
    SELECT 'Inserted' AS Outcome;
END

COMMIT;";

    public const string SetEnrollmentWindow = @"
SET XACT_ABORT ON;
BEGIN TRAN;

DECLARE @ts date, @te date;
SELECT @ts = StartDate, @te = EndDate FROM Terms WITH (READCOMMITTEDLOCK) WHERE Id=@term;
IF @ts IS NULL BEGIN SELECT 'NotFound' AS Outcome; ROLLBACK; RETURN; END;

-- validate window within term dates
IF (CAST(@startAt AS date) < @ts OR CAST(@endAt AS date) > @te OR @startAt >= @endAt)
BEGIN SELECT 'ValidationFailed' AS Outcome; ROLLBACK; RETURN; END;

IF EXISTS (SELECT 1 FROM EnrollmentWindows WITH (UPDLOCK, HOLDLOCK) WHERE TermId=@term)
BEGIN
    IF @rowver IS NOT NULL
    BEGIN
        UPDATE EnrollmentWindows SET StartAt=@startAt, EndAt=@endAt
        WHERE TermId=@term AND RowVersion=@rowver;
        IF @@ROWCOUNT = 0 BEGIN SELECT 'Conflict' AS Outcome; ROLLBACK; RETURN; END
    END
    ELSE
    BEGIN
        UPDATE EnrollmentWindows SET StartAt=@startAt, EndAt=@endAt WHERE TermId=@term;
    END
    SELECT 'Updated' AS Outcome;
END
ELSE
BEGIN
    INSERT INTO EnrollmentWindows (TermId, StartAt, EndAt) VALUES (@term, @startAt, @endAt);
    SELECT 'Inserted' AS Outcome;
END

COMMIT;";
}
