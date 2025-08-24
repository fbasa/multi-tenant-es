
namespace UniEnroll.Infrastructure.EF.Sql;

public static class InstructorSql
{
    public const string Upsert = @"
SET XACT_ABORT ON;
BEGIN TRAN;
IF EXISTS (SELECT 1 FROM Instructors WITH (UPDLOCK, HOLDLOCK) WHERE Id=@id)
BEGIN
    UPDATE Instructors SET FirstName=@first, LastName=@last, Email=@email WHERE Id=@id;
    SELECT 'Updated' AS Outcome;
END
ELSE
BEGIN
    INSERT INTO Instructors (Id, FirstName, LastName, Email, CreatedAt) VALUES (@id, @first, @last, @email, SYSUTCDATETIME());
    SELECT 'Inserted' AS Outcome;
END
COMMIT;";

    // Assign instructor by copying the section's schedule slots; prevents time overlap with existing assignments.
    public const string AssignToSection = @"
SET XACT_ABORT ON;
BEGIN TRAN;

-- conflict: overlapping slots with the same instructor
IF EXISTS (
  SELECT TOP(1) 1
  FROM ScheduleSlots newSs
  WHERE newSs.SectionId = @section
    AND EXISTS (
      SELECT 1
      FROM TeachingAssignments ta
      JOIN ScheduleSlots ss ON ss.SectionId = ta.SectionId
      WHERE ta.InstructorId = @instructor
        AND ss.DayOfWeek = newSs.DayOfWeek
        AND ss.StartTime < newSs.EndTime
        AND newSs.StartTime < ss.EndTime
    )
)
BEGIN
    SELECT 'ValidationFailed' AS Outcome;
    ROLLBACK; RETURN;
END

-- replace current assignments for the section
DELETE FROM TeachingAssignments WHERE SectionId=@section;

INSERT INTO TeachingAssignments (SectionId, InstructorId, DayOfWeek, StartTime, EndTime)
SELECT ss.SectionId, @instructor, ss.DayOfWeek, ss.StartTime, ss.EndTime
FROM ScheduleSlots ss WHERE ss.SectionId=@section;

COMMIT; SELECT 'Assigned' AS Outcome;";
}
