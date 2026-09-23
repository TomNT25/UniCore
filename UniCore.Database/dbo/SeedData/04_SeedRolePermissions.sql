-- Seed Data: dbo.role_permissions

DECLARE @AdminRoleId VARCHAR(50) = '018f4b5a-1a2b-7c3d-8e4f-5a6b7c8d9e0f';

INSERT INTO dbo.role_permissions (role_id, permission_id, assigned_at, is_active)
SELECT
    @AdminRoleId,
    p.id,
    GETDATE(),
    1
FROM dbo.permissions p
WHERE p.name IN (
    'GetAllStudent',
    'GetStudentByID',
    'AddStudent',
    'UpdateStudent',
    'DeleteStudent'
)
AND NOT EXISTS (
    SELECT 1
    FROM dbo.role_permissions rp
    WHERE rp.role_id = @AdminRoleId AND rp.permission_id = p.id
);
GO
