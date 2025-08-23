using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UniEnroll.Application.Abstractions;
using UniEnroll.Infrastructure.EF.Persistence;

namespace UniEnroll.Infrastructure.EF.Enrollment;

/// <summary>
/// Implements enrollment & drop rules via set-based SQL inside a single transaction.
/// This aligns with: prereq/coreq/credit validation; lock section row (UPDLOCK, ROWLOCK);
/// insert Enrolled vs Waitlisted; write EnrollmentAudit; use ROWVERSION on conflicts.
/// </summary>
internal sealed class SqlEnrollmentCommandRepository : IEnrollmentCommandRepository
{
    private readonly UniEnrollDbContext _db;
    public SqlEnrollmentCommandRepository(UniEnrollDbContext db) => _db = db;

    public async Task EnrollOrWaitlistAsync(string tenantId, string sectionId, string studentId, string enrollmentId, string? reason, CancellationToken ct = default)
    {
        // NOTE: For brevity, prereq/coreq checks are placeholders. Replace with actual set-based validations.
        var sql = @"
SET XACT_ABORT ON;
BEGIN TRAN;

-- Lock section row to compute seats
SELECT s.SeatsTaken, s.Capacity, s.Id
FROM Sections s WITH (UPDLOCK, ROWLOCK)
WHERE s.TenantId = @tenant AND s.Id = @section;

DECLARE @capacityTotal int, @waitlistCap int;
-- Capacity stored as 'total|waitlist'
SELECT @capacityTotal = TRY_CONVERT(int, PARSENAME(REPLACE(s.Capacity, '|', '.'), 2)),
       @waitlistCap   = TRY_CONVERT(int, PARSENAME(REPLACE(s.Capacity, '|', '.'), 1))
FROM Sections s WHERE s.TenantId = @tenant AND s.Id = @section;

DECLARE @seatsTaken int = (SELECT s.SeatsTaken FROM Sections s WHERE s.TenantId = @tenant AND s.Id = @section);
DECLARE @status nvarchar(16);

IF (@seatsTaken < @capacityTotal)
BEGIN
    SET @status = N'Enrolled';
    UPDATE Sections SET SeatsTaken = SeatsTaken + 1 WHERE TenantId = @tenant AND Id = @section;
END
ELSE
BEGIN
    SET @status = N'Waitlisted';
END

INSERT INTO Enrollments (Id, StudentId, SectionId, Status, TenantId)
VALUES (@enroll, @student, @section, @status, @tenant);

INSERT INTO EnrollmentAudits (Id, EnrollmentId, Action, ActorUserId, Reason, CreatedAt)
VALUES (CONCAT('AUD-', @enroll), @enroll, @status, @student, @reason, SYSUTCDATETIME());

COMMIT;";
        var p = new[]
        {
            new SqlParameter("@tenant", tenantId),
            new SqlParameter("@section", sectionId),
            new SqlParameter("@student", studentId),
            new SqlParameter("@enroll", enrollmentId),
            new SqlParameter("@reason", (object?)reason ?? (object)System.DBNull.Value)
        };
        await _db.Database.ExecuteSqlRawAsync(sql, p, ct);
    }

    public async Task DropAsync(string tenantId, string enrollmentId, string sectionId, byte[] rowVersion, string actorUserId, string? reason, CancellationToken ct = default)
    {
        var sql = @"
SET XACT_ABORT ON;
BEGIN TRAN;

-- Drop enrollment with optimistic concurrency
UPDATE e SET e.Status = N'Dropped'
FROM Enrollments e
WHERE e.TenantId = @tenant AND e.Id = @enroll AND e.RowVersion = @rowversion;

IF @@ROWCOUNT = 0
BEGIN
    RAISERROR ('Concurrency conflict', 16, 1);
END

-- Promote waitlist FIFO (simplified - first by CreatedAt)
DECLARE @promoteId nvarchar(64) = (
    SELECT TOP 1 Id FROM Enrollments
    WHERE TenantId = @tenant AND SectionId = @section AND Status = N'Waitlisted'
    ORDER BY CreatedAt
);

IF (@promoteId IS NOT NULL)
BEGIN
    UPDATE Enrollments SET Status = N'Enrolled' WHERE Id = @promoteId AND TenantId = @tenant;
END
ELSE
BEGIN
    -- Release a seat
    UPDATE Sections SET SeatsTaken = CASE WHEN SeatsTaken > 0 THEN SeatsTaken - 1 ELSE 0 END
    WHERE TenantId = @tenant AND Id = @section;
END

INSERT INTO EnrollmentAudits (Id, EnrollmentId, Action, ActorUserId, Reason, CreatedAt)
VALUES (CONCAT('AUD-', @enroll, '-DR'), @enroll, N'Dropped', @actor, @reason, SYSUTCDATETIME());

COMMIT;";
        var p = new[]
        {
            new SqlParameter("@tenant", tenantId),
            new SqlParameter("@enroll", enrollmentId),
            new SqlParameter("@section", sectionId),
            new SqlParameter("@rowversion", rowVersion) { SqlDbType = SqlDbType.Timestamp },
            new SqlParameter("@actor", actorUserId),
            new SqlParameter("@reason", (object?)reason ?? (object)System.DBNull.Value)
        };
        await _db.Database.ExecuteSqlRawAsync(sql, p, ct);
    }
}
