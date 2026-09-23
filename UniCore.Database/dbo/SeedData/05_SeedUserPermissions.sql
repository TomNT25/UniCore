-- Seed Data: dbo.user_permissions

DECLARE @AdminUserId VARCHAR(50) = '018f4b5a-2b3c-7d4e-8f5a-6b7c8d9e0f1a';

INSERT INTO dbo.user_permissions (user_id, permission_id, assigned_at, is_active)
SELECT
    @AdminUserId,
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
    FROM dbo.user_permissions up
    WHERE up.user_id = @AdminUserId AND up.permission_id = p.id
);
GO
