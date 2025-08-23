
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UniEnroll.Seeder.Seeders;

internal sealed class DemoAcademicDataSeeder : SeederBase
{
    private readonly IConfiguration _config;
    public DemoAcademicDataSeeder(ILogger<DemoAcademicDataSeeder> logger, IConfiguration config) : base(logger, config) { _config = config; }

    public override async Task SeedAsync(CancellationToken ct)
    {
        var tid = _config["Seeder:Tenant:Id"] ?? "default";

        // Ensure core tables exist (minimal columns to avoid schema mismatch)
        const string ensure = @"
IF OBJECT_ID('Terms','U') IS NULL BEGIN CREATE TABLE Terms (Id NVARCHAR(64) NOT NULL PRIMARY KEY, YearTermCode NVARCHAR(32) NOT NULL, Status NVARCHAR(32) NOT NULL); END
IF OBJECT_ID('Courses','U') IS NULL BEGIN CREATE TABLE Courses (Id NVARCHAR(64) NOT NULL PRIMARY KEY, Code NVARCHAR(32) NOT NULL, Title NVARCHAR(256) NOT NULL, Units INT NOT NULL, TenantId NVARCHAR(64) NOT NULL); END
IF OBJECT_ID('Sections','U') IS NULL BEGIN CREATE TABLE Sections (Id NVARCHAR(64) NOT NULL PRIMARY KEY, CourseId NVARCHAR(64) NOT NULL, TermId NVARCHAR(64) NOT NULL, InstructorId NVARCHAR(64) NULL, Capacity INT NOT NULL, TenantId NVARCHAR(64) NOT NULL, RowVersion ROWVERSION); END
";
        await ExecAsync(ensure, Array.Empty<SqlParameter>(), ct);

        // Insert a term
        await ExecAsync("IF NOT EXISTS (SELECT 1 FROM Terms WHERE Id='2025S1') INSERT INTO Terms (Id, YearTermCode, Status) VALUES ('2025S1', 'AY2025-S1', 'Published');", Array.Empty<SqlParameter>(), ct);

        // Insert a few demo courses
        foreach (var c in new[] {
            new { Id = "CMSC11", Code = "CMSC 11", Title = "Introduction to Computer Science", Units = 3 },
            new { Id = "MATH17", Code = "MATH 17", Title = "Algebra and Trigonometry", Units = 5 },
            new { Id = "ENG1", Code = "ENG 1", Title = "Basic English", Units = 3 },
        })
        {
            const string up = "IF NOT EXISTS (SELECT 1 FROM Courses WHERE Id=@id) INSERT INTO Courses (Id, Code, Title, Units, TenantId) VALUES (@id,@code,@title,@units,@tid);";
            await ExecAsync(up, new[] {
                P("@id", c.Id, SqlDbType.NVarChar, 64),
                P("@code", c.Code, SqlDbType.NVarChar, 32),
                P("@title", c.Title, SqlDbType.NVarChar, 256),
                P("@units", c.Units, SqlDbType.Int),
                P("@tid", tid, SqlDbType.NVarChar, 64)
            }, ct);
        }

        // Insert demo sections
        foreach (var s in new[] {
            new { Id = "CMSC11-A", CourseId = "CMSC11", TermId = "2025S1", Capacity = 40 },
            new { Id = "MATH17-A", CourseId = "MATH17", TermId = "2025S1", Capacity = 50 },
            new { Id = "ENG1-A", CourseId = "ENG1", TermId = "2025S1", Capacity = 35 },
        })
        {
            const string up = "IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id=@id) INSERT INTO Sections (Id, CourseId, TermId, Capacity, TenantId) VALUES (@id,@course,@term,@cap,@tid);";
            await ExecAsync(up, new[] {
                P("@id", s.Id, SqlDbType.NVarChar, 64),
                P("@course", s.CourseId, SqlDbType.NVarChar, 64),
                P("@term", s.TermId, SqlDbType.NVarChar, 64),
                P("@cap", s.Capacity, SqlDbType.Int),
                P("@tid", tid, SqlDbType.NVarChar, 64)
            }, ct);
        }

        _logger.LogInformation("DemoAcademicDataSeeder executed.");
    }
}
