
namespace UniEnroll.Infrastructure.EF.Sql;

public static class SchedulingSql
{
    public const string BuildTimetable = @"
SET XACT_ABORT ON;
BEGIN TRAN;

-- wipe existing
DELETE FROM Timetable WHERE StudentId=@student AND TermId=@term;

-- insert from enrolled sections
INSERT INTO Timetable (StudentId, TermId, SectionId, DayOfWeek, StartTime, EndTime, Room)
SELECT e.StudentId, @term, ss.SectionId, ss.DayOfWeek, ss.StartTime, ss.EndTime, ISNULL(ra.Room,'TBA')
FROM Enrollments e
JOIN ScheduleSlots ss ON ss.SectionId = e.SectionId
LEFT JOIN RoomAssignments ra ON ra.SectionId = ss.SectionId
WHERE e.StudentId=@student AND e.Status='Enrolled';

SELECT @@ROWCOUNT AS Created;
COMMIT;";

    public const string AssignRoomWithClashCheck = @"
SET XACT_ABORT ON;
BEGIN TRAN;

-- Detect conflict: same room, overlapping time, same day, different section
IF EXISTS (
  SELECT 1
  FROM ScheduleSlots sNew
  CROSS APPLY (SELECT @section AS SectionId, @room AS Room) AS desired
  JOIN ScheduleSlots s ON s.DayOfWeek = sNew.DayOfWeek
  JOIN RoomAssignments ra ON ra.SectionId = s.SectionId
  WHERE sNew.SectionId = desired.SectionId
    AND ra.Room = desired.Room
    AND s.SectionId <> desired.SectionId
    AND s.StartTime < sNew.EndTime AND sNew.StartTime < s.EndTime
)
BEGIN SELECT 'Conflict' AS Outcome; ROLLBACK; RETURN; END;

IF EXISTS (SELECT 1 FROM RoomAssignments WITH (UPDLOCK, HOLDLOCK) WHERE SectionId=@section)
    UPDATE RoomAssignments SET Room=@room WHERE SectionId=@section;
ELSE
    INSERT INTO RoomAssignments (SectionId, Room) VALUES (@section, @room);

SELECT 'Success' AS Outcome;
COMMIT;";

    public const string DetectRoomConflicts = @"
SET XACT_ABORT ON;
BEGIN TRAN;

IF OBJECT_ID('SchedulingConflicts','U') IS NULL
BEGIN
  CREATE TABLE SchedulingConflicts(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TermId UNIQUEIDENTIFIER NOT NULL,
    SectionId UNIQUEIDENTIFIER NOT NULL,
    ConflictsWithSectionId UNIQUEIDENTIFIER NOT NULL,
    Room NVARCHAR(32) NOT NULL,
    DayOfWeek INT NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    CreatedAt DATETIMEOFFSET(7) NOT NULL
  );
  CREATE INDEX IX_SchedulingConflicts_Term ON SchedulingConflicts(TermId);
END

DELETE FROM SchedulingConflicts WHERE TermId=@term;

INSERT INTO SchedulingConflicts (TermId, SectionId, ConflictsWithSectionId, Room, DayOfWeek, StartTime, EndTime, CreatedAt)
SELECT @term, a.SectionId, b.SectionId, ra.Room, a.DayOfWeek, a.StartTime, a.EndTime, SYSUTCDATETIME()
FROM ScheduleSlots a
JOIN ScheduleSlots b ON a.DayOfWeek = b.DayOfWeek
JOIN RoomAssignments ra ON ra.SectionId = a.SectionId
JOIN RoomAssignments rb ON rb.SectionId = b.SectionId AND rb.Room = ra.Room
WHERE a.SectionId <> b.SectionId
  AND a.StartTime < b.EndTime AND b.StartTime < a.EndTime;

SELECT @@ROWCOUNT AS Conflicts;
COMMIT;";

    public const string GetStudentSchedule = @"
SELECT t.SectionId, c.CourseCode, t.Room, t.DayOfWeek, CONVERT(varchar(8), t.StartTime, 108) AS StartTime, CONVERT(varchar(8), t.EndTime, 108) AS EndTime
FROM Timetable t
JOIN Sections s ON s.Id = t.SectionId
JOIN Courses c ON c.Id = s.CourseId
WHERE t.StudentId=@student AND t.TermId=@term
ORDER BY t.DayOfWeek, t.StartTime;";

    public const string ListRoomConflicts = @"
SELECT SectionId, Room, DayOfWeek, CONVERT(varchar(8), StartTime, 108) AS StartTime, CONVERT(varchar(8), EndTime, 108) AS EndTime, ConflictsWithSectionId
FROM SchedulingConflicts WHERE TermId=@term";
}
