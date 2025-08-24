
namespace UniEnroll.Infrastructure.EF.Sql;

public static class IdentitySql
{
    public const string SelectUserByEmailTenant = @"
SELECT TOP (1) u.Id, u.Email, u.TenantId, u.PasswordHash, u.PasswordSalt
FROM Users u WITH (READCOMMITTEDLOCK)
WHERE u.Email = @email AND u.TenantId = @tenant;";

    public const string SelectRolesByUser = @"
SELECT ur.RoleId
FROM UserRoles ur WITH (READCOMMITTEDLOCK)
WHERE ur.UserId = @userId;";

    public const string EnsureUsersAndUserRoles = @"
IF OBJECT_ID('Users','U') IS NULL
BEGIN
    CREATE TABLE Users (
        Id NVARCHAR(64) NOT NULL PRIMARY KEY,
        Email NVARCHAR(256) NOT NULL UNIQUE,
        FirstName NVARCHAR(64) NULL,
        LastName NVARCHAR(64) NULL,
        TenantId NVARCHAR(64) NOT NULL,
        PasswordHash VARBINARY(64) NULL,
        PasswordSalt VARBINARY(16) NULL
    );
END
IF OBJECT_ID('UserRoles','U') IS NULL
BEGIN
    CREATE TABLE UserRoles (
        UserId NVARCHAR(64) NOT NULL,
        RoleId NVARCHAR(64) NOT NULL,
        CONSTRAINT PK_UserRoles PRIMARY KEY (UserId, RoleId)
    );
END";
}
