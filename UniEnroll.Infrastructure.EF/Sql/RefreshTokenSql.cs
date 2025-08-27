
namespace UniEnroll.Infrastructure.EF.Sql;

public static class RefreshTokenSql
{
    public const string EnsureTable = @"
IF OBJECT_ID('RefreshTokens','U') IS NULL
BEGIN
  CREATE TABLE RefreshTokens(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    TenantId NVARCHAR(64) NOT NULL,
    UserId NVARCHAR(64) NOT NULL,
    TokenHash VARBINARY(32) NOT NULL UNIQUE,
    DeviceId NVARCHAR(128) NULL,
    ExpiresAt DATETIMEOFFSET(7) NOT NULL,
    CreatedAt DATETIMEOFFSET(7) NOT NULL,
    CreatedByIp NVARCHAR(64) NULL,
    RevokedAt DATETIMEOFFSET(7) NULL,
    RevokedByIp NVARCHAR(64) NULL,
    ReasonRevoked NVARCHAR(128) NULL,
    ReplacedByTokenId UNIQUEIDENTIFIER NULL
  );
  CREATE INDEX IX_RefreshTokens_User ON RefreshTokens(UserId);
  CREATE INDEX IX_RefreshTokens_Hash ON RefreshTokens(TokenHash);
END";

    public const string Insert = @"
INSERT INTO RefreshTokens (TenantId, UserId, TokenHash, DeviceId, ExpiresAt, CreatedAt, CreatedByIp)
VALUES (@t, @u, @h, @d, @exp, @now, @ip);";

    public const string SelectByHash = @"
SELECT TOP (1) Id, TenantId, UserId, DeviceId, ExpiresAt, RevokedAt, ReplacedByTokenId
FROM RefreshTokens WITH (READCOMMITTEDLOCK)
WHERE TokenHash = @h;";

    public const string RevokeOnRotate = @"
UPDATE RefreshTokens SET RevokedAt=@now, RevokedByIp=@ip, ReasonRevoked='rotated' WHERE Id=@id;
UPDATE RefreshTokens SET ReplacedByTokenId = (SELECT TOP(1) Id FROM RefreshTokens WHERE TokenHash=@hNew) WHERE Id=@id;";

    public const string RevokeByHash = @"
UPDATE RefreshTokens SET RevokedAt=@now, RevokedByIp=@ip, ReasonRevoked=@reason WHERE TokenHash=@h;";

    public const string RevokeAllForUser = @"
UPDATE RefreshTokens SET RevokedAt=@now, RevokedByIp=@ip, ReasonRevoked=@reason
WHERE TenantId=@t AND UserId=@u AND RevokedAt IS NULL;";

    public const string RevokeChain = @"
UPDATE RefreshTokens SET RevokedAt=@now, RevokedByIp=@ip, ReasonRevoked=@reason
WHERE Id=@id OR ReplacedByTokenId=@id;";
}
