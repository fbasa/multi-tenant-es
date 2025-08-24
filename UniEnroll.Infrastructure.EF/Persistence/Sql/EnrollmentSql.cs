using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace UniEnroll.Infrastructure.EF.Persistence.Sql;

public static class EnrollmentSql
{
    public static async Task TryEnrollOrWaitlistAsync(UniEnrollDbContext ctx, string tenantId, string sectionId, string studentId, string enrollmentId, string? reason, CancellationToken ct)
    {
        var sql = @"
-- lock section row
SELECT SeatsTotal = CapacityTotal, s.SeatsTaken, s.CapacityWaitlist
FROM dbo.Sections s WITH (UPDLOCK, ROWLOCK)
WHERE s.SectionId = @SectionId AND s.TenantId = @TenantId;

IF (@@ROWCOUNT = 0) THROW 50000, 'Section not found', 1;

DECLARE @cap INT, @taken INT, @waitcap INT;
SELECT @cap = CapacityTotal, @taken = SeatsTaken, @waitcap = CapacityWaitlist
FROM dbo.Sections WHERE SectionId=@SectionId AND TenantId=@TenantId;

IF (@taken < @cap)
BEGIN
  UPDATE dbo.Sections SET SeatsTaken = SeatsTaken + 1 WHERE SectionId=@SectionId AND TenantId=@TenantId;
  INSERT dbo.Enrollments (EnrollmentId, StudentId, SectionId, Status, TenantId)
  VALUES (@EnrollmentId, @StudentId, @SectionId, 'Enrolled', @TenantId);

  INSERT dbo.EnrollmentAudit (EnrollmentId, ActorUserId, Action, Reason, CreatedAt, TenantId)
  VALUES (@EnrollmentId, @StudentId, 'Enroll', @Reason, SYSUTCDATETIME(), @TenantId);
END
ELSE
BEGIN
  DECLARE @wlCount INT = (SELECT COUNT(1) FROM dbo.Waitlist WITH (UPDLOCK, ROWLOCK) WHERE SectionId=@SectionId AND TenantId=@TenantId);
  IF (@wlCount < @waitcap)
  BEGIN
    DECLARE @WaitlistId NVARCHAR(64) = REPLACE(CONVERT(NVARCHAR(36), NEWID()), '-', '');
    INSERT dbo.Waitlist (WaitlistId, StudentId, SectionId, CreatedAt, TenantId)
    VALUES (@WaitlistId, @StudentId, @SectionId, SYSUTCDATETIME(), @TenantId);

    INSERT dbo.Enrollments (EnrollmentId, StudentId, SectionId, Status, TenantId)
    VALUES (@EnrollmentId, @StudentId, @SectionId, 'Waitlisted', @TenantId);

    INSERT dbo.EnrollmentAudit (EnrollmentId, ActorUserId, Action, Reason, CreatedAt, TenantId)
    VALUES (@EnrollmentId, @StudentId, 'Waitlist', @Reason, SYSUTCDATETIME(), @TenantId);
  END
  ELSE
    THROW 50001, 'No seats or waitlist slots available', 1;
END
";
        var conn = (SqlConnection)ctx.Database.GetDbConnection();
        await EnsureOpenAsync(conn, ct);

        using var cmd = new SqlCommand(sql, conn, (SqlTransaction?)ctx.Database.CurrentTransaction?.GetDbTransaction());
        cmd.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        cmd.Parameters.Add(new SqlParameter("@SectionId", sectionId));
        cmd.Parameters.Add(new SqlParameter("@StudentId", studentId));
        cmd.Parameters.Add(new SqlParameter("@EnrollmentId", enrollmentId));
        cmd.Parameters.Add(new SqlParameter("@Reason", (object?)reason ?? DBNull.Value));
        await cmd.ExecuteNonQueryAsync(ct);
    }

    public static async Task DropAndPromoteAsync(UniEnrollDbContext ctx, string tenantId, string enrollmentId, string sectionId, byte[] rowVersion, string actorUserId, string? reason, CancellationToken ct)
    {
        var sql = @"
-- lock section
SELECT s.SeatsTaken FROM dbo.Sections s WITH (UPDLOCK, ROWLOCK)
WHERE s.SectionId=@SectionId AND s.TenantId=@TenantId;

-- transition enrollment
UPDATE dbo.Enrollments SET Status='Dropped'
WHERE EnrollmentId=@EnrollmentId AND TenantId=@TenantId AND RowVersion=@RowVersion;

IF (@@ROWCOUNT = 0) THROW 50002, 'Concurrency conflict', 1;

-- promote top waitlist if exists
DECLARE @PromotedWaitlistId NVARCHAR(64) =
  (SELECT TOP (1) WaitlistId FROM dbo.Waitlist WITH (UPDLOCK, ROWLOCK)
   WHERE SectionId=@SectionId AND TenantId=@TenantId ORDER BY CreatedAt ASC);

IF (@PromotedWaitlistId IS NOT NULL)
BEGIN
  DELETE dbo.Waitlist WHERE WaitlistId=@PromotedWaitlistId AND TenantId=@TenantId;

  UPDATE e SET Status='Enrolled'
  FROM dbo.Enrollments e
  WHERE e.TenantId=@TenantId AND e.SectionId=@SectionId AND e.StudentId =
    (SELECT w.StudentId FROM dbo.Waitlist w WITH (READPAST) WHERE w.WaitlistId=@PromotedWaitlistId);

  INSERT dbo.EnrollmentAudit (EnrollmentId, ActorUserId, Action, Reason, CreatedAt, TenantId)
  VALUES (@EnrollmentId, @ActorUserId, 'PromoteWaitlist', @Reason, SYSUTCDATETIME(), @TenantId);
END
ELSE
BEGIN
  UPDATE dbo.Sections SET SeatsTaken = SeatsTaken - 1 WHERE SectionId=@SectionId AND TenantId=@TenantId;
END

INSERT dbo.EnrollmentAudit (EnrollmentId, ActorUserId, Action, Reason, CreatedAt, TenantId)
VALUES (@EnrollmentId, @ActorUserId, 'Drop', @Reason, SYSUTCDATETIME(), @TenantId);
";
        var conn = (SqlConnection)ctx.Database.GetDbConnection();
        await EnsureOpenAsync(conn, ct);

        using var cmd = new SqlCommand(sql, conn, (SqlTransaction?)ctx.Database.CurrentTransaction?.GetDbTransaction());
        cmd.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        cmd.Parameters.Add(new SqlParameter("@SectionId", sectionId));
        cmd.Parameters.Add(new SqlParameter("@EnrollmentId", enrollmentId));
        cmd.Parameters.Add(new SqlParameter("@RowVersion", rowVersion));
        cmd.Parameters.Add(new SqlParameter("@ActorUserId", actorUserId));
        cmd.Parameters.Add(new SqlParameter("@Reason", (object?)reason ?? DBNull.Value));
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static async Task EnsureOpenAsync(SqlConnection conn, CancellationToken ct)
    {
        if (conn.State != ConnectionState.Open) await conn.OpenAsync(ct);
    }
}
