-- 1. Declare static IDs so we can link the User to the Role and related entities
DECLARE @AdminRoleId VARCHAR(50) = '018f4b5a-1a2b-7c3d-8e4f-5a6b7c8d9e0f';
DECLARE @AdminUserId VARCHAR(50) = '018f4b5a-2b3c-7d4e-8f5a-6b7c8d9e0f1a';

-- 2. Insert the 'Admin' Role
IF NOT EXISTS (SELECT 1 FROM roles WHERE id = @AdminRoleId)
BEGIN
    INSERT INTO roles (
        id, code, name, description, is_active, is_system_role, created_at, updated_at
    )
    VALUES (
        @AdminRoleId,
        'ROLE_ADMIN',
        'System Administrator',
        'Has full access to the system',
        1,
        1,
        GETDATE(),
        GETDATE()
    );
END

-- 3. Insert the Admin User (Auth Core)
IF NOT EXISTS (SELECT 1 FROM users WHERE email = 'admin@example.com')
BEGIN
    INSERT INTO users (
        id, code, username, email, password_hash,
        provider, role_id, is_active, is_email_verified, created_at, updated_at
    )
    VALUES (
        @AdminUserId,
        'USR-0001',
        'admin',
        'admin@example.com',

        -- BCrypt hash for 'Password123!'
        '$2y$10$dmqs2ksXAlQHrq.hIK7nQ.5DEiUAKYW3oBFDHvOgejCLQEAJX2VJ2',

        'system',
        @AdminRoleId,
        1,
        1,
        GETDATE(),
        GETDATE()
    );
END

-- 4. Seed User Profile (1:1 with User)
IF NOT EXISTS (SELECT 1 FROM user_profiles WHERE user_id = @AdminUserId)
BEGIN
    INSERT INTO user_profiles (
        code, user_id, first_name, last_name, full_name, phone_number,
        gender, birth_date, address, bio, is_active, created_at, updated_at
    )
    VALUES (
        'PRF-0001',
        @AdminUserId,
        N'Quang',
        N'Nguyen',
        N'Nguyen Van Quang',
        '0901234567',
        'MALE',
        '1995-05-15',
        N'Ha Noi, Viet Nam',
        N'System Administrator & Lead Software Engineer',
        1,
        GETDATE(),
        GETDATE()
    );
END

-- 5. Seed Vietnam Person ID (CCCD - Identity Verification)
IF NOT EXISTS (SELECT 1 FROM user_person_ids WHERE user_id = @AdminUserId)
BEGIN
    INSERT INTO user_person_ids (
        code, user_id, id_number, full_name, card_type, birth_date, gender,
        nationality, place_of_origin, place_of_residence, issue_date, expire_date,
        issue_place, verification_status, verified_at, is_active, created_at, updated_at
    )
    VALUES (
        'CCCD-0001',
        @AdminUserId,
        '001095012345',
        N'NGUYỄN VĂN QUANG',
        'CCCD_CHIP',
        '1995-05-15',
        N'Nam',
        N'Việt Nam',
        N'Phường Hàng Bạc, Quận Hoàn Kiếm, Thành phố Hà Nội',
        N'Số 15 Phố Lý Thường Kiệt, Quận Hoàn Kiếm, Thành phố Hà Nội',
        '2021-08-10',
        '2035-05-15',
        N'Cục Cảnh sát quản lý hành chính về trật tự xã hội',
        'VERIFIED',
        GETDATE(),
        1,
        GETDATE(),
        GETDATE()
    );
END

-- 6. Seed Google OAuth 2.0 Simulation Provider
IF NOT EXISTS (SELECT 1 FROM user_external_logins WHERE user_id = @AdminUserId AND provider = 'GOOGLE')
BEGIN
    INSERT INTO user_external_logins (
        code, user_id, provider, provider_user_id, provider_email, provider_display_name,
        avatar_url, is_active, created_at, updated_at
    )
    VALUES (
        'OAUTH-GGL-001',
        @AdminUserId,
        'GOOGLE',
        '109283746519283746501',
        'admin@example.com',
        N'Nguyen Van Quang (Google)',
        'https://lh3.googleusercontent.com/a/default-user-avatar',
        1,
        GETDATE(),
        GETDATE()
    );
END

-- 7. Seed MFA Settings
IF NOT EXISTS (SELECT 1 FROM user_mfa_settings WHERE user_id = @AdminUserId)
BEGIN
    INSERT INTO user_mfa_settings (
        code, user_id, mfa_method, secret_key, is_mfa_enabled, enabled_at, is_active, created_at, updated_at
    )
    VALUES (
        'MFA-0001',
        @AdminUserId,
        'TOTP',
        'JBSWY3DPEHPK3PXP', -- Base32 encoded sample TOTP secret key
        1,
        GETDATE(),
        1,
        GETDATE(),
        GETDATE()
    );
END
GO

DECLARE @AdminRoleId VARCHAR(50) = '018f4b5a-1a2b-7c3d-8e4f-5a6b7c8d9e0f';
DECLARE @AdminUserId VARCHAR(50) = '018f4b5a-2b3c-7d4e-8f5a-6b7c8d9e0f1a';

-- 8. Insert Permissions
IF NOT EXISTS (SELECT 1 FROM permissions WHERE name = 'GetAllStudent')
BEGIN
    INSERT INTO permissions (code, name, description, resource, action, is_active)
    VALUES ('PERM_STU_GET_ALL', 'GetAllStudent', 'View list of all students', 'STUDENT', 'GET_ALL', 1);
END

IF NOT EXISTS (SELECT 1 FROM permissions WHERE name = 'GetStudentByID')
BEGIN
    INSERT INTO permissions (code, name, description, resource, action, is_active)
    VALUES ('PERM_STU_GET_BY_ID', 'GetStudentByID', 'View details of a specific student', 'STUDENT', 'GET_BY_ID', 1);
END

IF NOT EXISTS (SELECT 1 FROM permissions WHERE name = 'AddStudent')
BEGIN
    INSERT INTO permissions (code, name, description, resource, action, is_active)
    VALUES ('PERM_STU_ADD', 'AddStudent', 'Add a new student to the system', 'STUDENT', 'ADD', 1);
END

IF NOT EXISTS (SELECT 1 FROM permissions WHERE name = 'UpdateStudent')
BEGIN
    INSERT INTO permissions (code, name, description, resource, action, is_active)
    VALUES ('PERM_STU_UPDATE', 'UpdateStudent', 'Update an existing student''s information', 'STUDENT', 'UPDATE', 1);
END

IF NOT EXISTS (SELECT 1 FROM permissions WHERE name = 'DeleteStudent')
BEGIN
    INSERT INTO permissions (code, name, description, resource, action, is_active)
    VALUES ('PERM_STU_DELETE', 'DeleteStudent', 'Remove a student from the system', 'STUDENT', 'DELETE', 1);
END

-- 9. Map Permissions to Admin Role
INSERT INTO role_permissions (role_id, permission_id, assigned_at, is_active)
SELECT
    @AdminRoleId, 
    p.id, 
    GETDATE(), 
    1
FROM permissions p
WHERE p.name IN (
    'GetAllStudent', 
    'GetStudentByID', 
    'AddStudent', 
    'UpdateStudent', 
    'DeleteStudent'
)
AND NOT EXISTS (
    SELECT 1 
    FROM role_permissions rp 
    WHERE rp.role_id = @AdminRoleId AND rp.permission_id = p.id
);

GO

DECLARE @AdminUserId VARCHAR(50) = '018f4b5a-2b3c-7d4e-8f5a-6b7c8d9e0f1a';

-- 10. Map Direct User Permissions
INSERT INTO user_permissions (
    user_id, 
    permission_id, 
    assigned_at, 
    is_active
)
SELECT 
    @AdminUserId, 
    p.id, 
    GETDATE(), 
    1
FROM permissions p
WHERE p.name IN (
    'GetAllStudent', 
    'GetStudentByID', 
    'AddStudent', 
    'UpdateStudent', 
    'DeleteStudent'
)
AND NOT EXISTS (
    SELECT 1 
    FROM user_permissions up 
    WHERE up.user_id = @AdminUserId 
      AND up.permission_id = p.id
);

GO