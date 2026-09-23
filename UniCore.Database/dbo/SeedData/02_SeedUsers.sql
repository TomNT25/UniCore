PRINT 'Seeding Users & User Profiles...';

-- Admin User
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'admin')
BEGIN
    INSERT INTO [dbo].[users] ([code], [username], [email], [password_hash], [provider], [role_id], [is_active], [is_email_verified])
    VALUES ('USR-00001', 'admin', 'admin@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 'role-admin-001', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([user_id], [first_name], [last_name], [full_name], [avatar_url], [gender], [birth_date])
    VALUES ('System', 'Admin', 'Administrator', 'https://avatar.iran.liara.run/public/1', 'Male', '1990-01-01');
END;

-- Lecturer User 1
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'lecturer1')
BEGIN
    INSERT INTO [dbo].[users] ([code], [username], [email], [password_hash], [provider], [role_id], [is_active], [is_email_verified])
    VALUES ('USR-00002', 'lecturer1', 'lecturer1@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 'role-lecturer-002', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([user_id], [first_name], [last_name], [full_name], [gender], [birth_date])
    VALUES ('usr-lecturer-001', 'Dr. Nguyen', 'Vinh', 'Dr. Nguyen Vinh', 'Male', '1985-05-15');
END;

-- Student User 1
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student1')
BEGIN
    INSERT INTO [dbo].[users] ([code], [username], [email], [password_hash], [provider], [role_id], [is_active], [is_email_verified])
    VALUES ('USR-00003', 'student1', 'student1@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 'role-student-003', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([user_id], [first_name], [last_name], [full_name], [gender], [birth_date])
    VALUES ('usr-student-001', 'Tran', 'Van A', 'Tran Van A', 'Male', '2003-08-20');
END;

-- Student User 2
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student2')
BEGIN
    INSERT INTO [dbo].[users] ([code], [username], [email], [password_hash], [provider], [role_id], [is_active], [is_email_verified])
    VALUES ('usr-student-002', 'USR-00004', 'student2', 'student2@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 'role-student-003', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([user_id], [first_name], [last_name], [full_name], [gender], [birth_date])
    VALUES ('prof-student-002', 'usr-student-002', 'Le', 'Thi B', 'Le Thi B', 'Female', '2003-11-12');
END;

GO
