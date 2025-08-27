
namespace UniEnroll.Infrastructure.EF.Sql;

public static class PermissionSql
{
    public const string HasPermission = @"
SELECT TOP (1) 1
FROM Users u WITH (READCOMMITTEDLOCK)
JOIN UserRoles ur ON ur.UserId = u.Id
JOIN RolePermissions rp ON rp.RoleId = ur.RoleId
WHERE u.Id = @user AND u.TenantId = @tenant AND rp.Permission = @perm;";
}
