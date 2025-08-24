
namespace UniEnroll.Infrastructure.EF.Sql;

public static class EnrollmentSql
{
    public const string ReserveSeat = @"
SET XACT_ABORT ON;
BEGIN TRAN;
IF EXISTS (SELECT 1 FROM Enrollments WITH (READCOMMITTEDLOCK) WHERE SectionId=@section AND StudentId=@student AND Status IN ('Enrolled','Waitlisted'))
BEGIN SELECT 'AlreadyEnrolled' AS Outcome; COMMIT; RETURN; END;
SELECT Capacity, WaitlistCapacity, SeatsTaken, WaitlistCount
INTO #S FROM Sections WITH (UPDLOCK, ROWLOCK) WHERE Id=@section;
IF NOT EXISTS (SELECT 1 FROM #S) BEGIN SELECT 'NotFound' AS Outcome; COMMIT; RETURN; END;
DECLARE @cap int=(SELECT Capacity FROM #S), @wlcap int=(SELECT WaitlistCapacity FROM #S),
        @taken int=(SELECT SeatsTaken FROM #S), @wlcnt int=(SELECT WaitlistCount FROM #S);
IF (@taken < @cap)
BEGIN
    INSERT INTO SeatReservations (SectionId, StudentId, ExpiresAt, CreatedAt)
    VALUES (@section, @student, DATEADD(MINUTE, @ttlMinutes, SYSUTCDATETIME()), SYSUTCDATETIME());
    UPDATE Sections SET SeatsTaken = SeatsTaken + 1 WHERE Id=@section;
    SELECT 'Enrolled' AS Outcome, CAST(SCOPE_IDENTITY() AS bigint) AS ReservationId, DATEADD(MINUTE, @ttlMinutes, SYSUTCDATETIME()) AS ExpiresAt;
    COMMIT; RETURN;
END
ELSE IF (@wlcnt < @wlcap)
BEGIN
    INSERT INTO WaitlistEntries (SectionId, StudentId, Position, CreatedAt)
    VALUES (@section, @student, @wlcnt + 1, SYSUTCDATETIME());
    UPDATE Sections SET WaitlistCount = WaitlistCount + 1 WHERE Id=@section;
    SELECT 'Waitlisted' AS Outcome, NULL AS ReservationId, NULL AS ExpiresAt;
    COMMIT; RETURN;
END
ELSE BEGIN SELECT 'NoSeats' AS Outcome; COMMIT; RETURN; END";

    public const string Enroll = @"
SET XACT_ABORT ON;
BEGIN TRAN;
IF EXISTS (SELECT 1 FROM Enrollments WITH (READCOMMITTEDLOCK) WHERE SectionId=@section AND StudentId=@student AND Status='Enrolled')
BEGIN SELECT 'AlreadyEnrolled' AS Outcome, NULL AS EnrollmentId; COMMIT; RETURN; END;
DECLARE @resId bigint = (SELECT TOP(1) Id FROM SeatReservations WITH (UPDLOCK, ROWLOCK)
                          WHERE SectionId=@section AND StudentId=@student AND ExpiresAt > SYSUTCDATETIME()
                          ORDER BY CreatedAt DESC);
IF (@resId IS NOT NULL) DELETE FROM SeatReservations WHERE Id=@resId;
ELSE
BEGIN
    DECLARE @cap int, @taken int;
    SELECT @cap=Capacity, @taken=SeatsTaken FROM Sections WITH (UPDLOCK, ROWLOCK) WHERE Id=@section;
    IF (@taken >= @cap) BEGIN SELECT 'NoSeats' AS Outcome, NULL AS EnrollmentId; COMMIT; RETURN; END;
    UPDATE Sections SET SeatsTaken = SeatsTaken + 1 WHERE Id=@section;
END
DECLARE @newId uniqueidentifier = NEWID();
INSERT INTO Enrollments (Id, SectionId, StudentId, Status, CreatedAt)
VALUES (@newId, @section, @student, 'Enrolled', SYSUTCDATETIME());
INSERT INTO EnrollmentAudit (EnrollmentId, Action, PerformedAt, PerformedBy, Reason)
VALUES (@newId, 'Enroll', SYSUTCDATETIME(), @userId, @reason);
SELECT 'Enrolled' AS Outcome, @newId AS EnrollmentId;
COMMIT; RETURN;";

    public const string Drop = @"
SET XACT_ABORT ON;
BEGIN TRAN;
DECLARE @secId uniqueidentifier = (SELECT SectionId FROM Enrollments WITH (UPDLOCK, ROWLOCK) WHERE Id=@enrollmentId AND Status='Enrolled');
IF (@secId IS NULL) BEGIN SELECT 'NotFound' AS Outcome, CAST(0 AS bit) AS Promoted; COMMIT; RETURN; END;
UPDATE Enrollments SET Status='Dropped', DroppedAt=SYSUTCDATETIME() WHERE Id=@enrollmentId AND Status='Enrolled';
IF @@ROWCOUNT = 0 BEGIN SELECT 'Conflict' AS Outcome, CAST(0 AS bit) AS Promoted; ROLLBACK; RETURN; END;
INSERT INTO EnrollmentAudit (EnrollmentId, Action, PerformedAt, PerformedBy, Reason)
VALUES (@enrollmentId, 'Drop', SYSUTCDATETIME(), @userId, @reason);
DECLARE @studentId nvarchar(64) = (SELECT TOP(1) StudentId FROM WaitlistEntries WITH (UPDLOCK, ROWLOCK) WHERE SectionId=@secId ORDER BY Position ASC);
IF (@studentId IS NULL)
BEGIN
    UPDATE Sections SET SeatsTaken = CASE WHEN SeatsTaken>0 THEN SeatsTaken-1 ELSE 0 END WHERE Id=@secId;
    SELECT 'Enrolled' AS Outcome, CAST(0 AS bit) AS Promoted; COMMIT; RETURN;
END
DELETE TOP(1) FROM WaitlistEntries WHERE SectionId=@secId AND StudentId=@studentId;
DECLARE @newId uniqueidentifier = NEWID();
INSERT INTO Enrollments (Id, SectionId, StudentId, Status, CreatedAt)
VALUES (@newId, @secId, @studentId, 'Enrolled', SYSUTCDATETIME());
INSERT INTO EnrollmentAudit (EnrollmentId, Action, PerformedAt, PerformedBy, Reason)
VALUES (@newId, 'PromotedFromWaitlist', SYSUTCDATETIME(), @userId, 'FIFO promotion after drop');
SELECT 'Enrolled' AS Outcome, CAST(1 AS bit) AS Promoted;
COMMIT; RETURN;";
}
