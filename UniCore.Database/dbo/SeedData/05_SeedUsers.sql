PRINT 'Seeding Users, Profiles, User Roles, Person IDs, MFA, Tokens, External Logins & Auth Logs...';

-- 1. Admin User & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'admin')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-admin-001', 'USR-00001', 'admin', 'admin@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [avatar_url], [gender], [birth_date])
    VALUES ('prof-admin-001', 'PRF-00001', 'usr-admin-001', 'System', 'Admin', 'System Administrator', '0901234567', 'https://avatar.iran.liara.run/public/1', 'Male', '1990-01-01');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-admin-001', 'UR-00001', 'usr-admin-001', 'role-admin-001', sysutcdatetime(), 1);
END;

-- 2. Lecturer User 1 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'lecturer1')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-lecturer-001', 'USR-00002', 'lecturer1', 'lecturer1@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-lecturer-001', 'PRF-00002', 'usr-lecturer-001', 'Vinh', 'Nguyen', 'Dr. Nguyen Vinh', '0912345678', 'Male', '1985-05-15');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-lecturer-001', 'UR-00002', 'usr-lecturer-001', 'role-lecturer-002', sysutcdatetime(), 1);
END;

-- 3. Student User 1 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student1')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [student_code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-student-001', 'USR-00003', 'STD-00001', 'student1', 'student1@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-student-001', 'PRF-00003', 'usr-student-001', 'Van A', 'Tran', 'Tran Van A', '0987654321', 'Male', '2003-08-20');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-student-001', 'UR-00003', 'usr-student-001', 'role-student-003', sysutcdatetime(), 1);
END;

-- 4. Student User 2 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student2')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [student_code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-student-002', 'USR-00004', 'STD-00002', 'student2', 'student2@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-student-002', 'PRF-00004', 'usr-student-002', 'Thi B', 'Le', 'Le Thi B', '0976543210', 'Female', '2003-11-12');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-student-002', 'UR-00004', 'usr-student-002', 'role-student-003', sysutcdatetime(), 1);
END;

-- 5. Lecturer User 2 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'lecturer2')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-lecturer-002', 'USR-00005', 'lecturer2', 'lecturer2@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-lecturer-002', 'PRF-00005', 'usr-lecturer-002', 'Hoang', 'Le', 'Dr. Le Hoang', '0911223344', 'Male', '1982-03-10');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-lecturer-002', 'UR-00005', 'usr-lecturer-002', 'role-lecturer-002', sysutcdatetime(), 1);
END;

-- 6. Lecturer User 3 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'lecturer3')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-lecturer-003', 'USR-00006', 'lecturer3', 'lecturer3@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-lecturer-003', 'PRF-00006', 'usr-lecturer-003', 'Minh', 'Pham', 'Dr. Pham Minh', '0922334455', 'Male', '1980-07-25');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-lecturer-003', 'UR-00006', 'usr-lecturer-003', 'role-lecturer-002', sysutcdatetime(), 1);
END;

-- 7. Student User 3 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student3')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [student_code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-student-003', 'USR-00007', 'STD-00003', 'student3', 'student3@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-student-003', 'PRF-00007', 'usr-student-003', 'Van C', 'Hoang', 'Hoang Van C', '0965432109', 'Male', '2004-01-15');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-student-003', 'UR-00007', 'usr-student-003', 'role-student-003', sysutcdatetime(), 1);
END;

-- 8. Student User 4 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student4')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [student_code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-student-004', 'USR-00008', 'STD-00004', 'student4', 'student4@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-student-004', 'PRF-00008', 'usr-student-004', 'Thi D', 'Pham', 'Pham Thi D', '0954321098', 'Female', '2004-04-18');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-student-004', 'UR-00008', 'usr-student-004', 'role-student-003', sysutcdatetime(), 1);
END;

-- 9. Student User 5 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student5')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [student_code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-student-005', 'USR-00009', 'STD-00005', 'student5', 'student5@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-student-005', 'PRF-00009', 'usr-student-005', 'Van E', 'Nguyen', 'Nguyen Van E', '0943210987', 'Male', '2004-09-05');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-student-005', 'UR-00009', 'usr-student-005', 'role-student-003', sysutcdatetime(), 1);
END;

-- 10. Student User 6 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student6')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [student_code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-student-006', 'USR-00010', 'STD-00006', 'student6', 'student6@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-student-006', 'PRF-00010', 'usr-student-006', 'Van F', 'Vo', 'Vo Van F', '0932109876', 'Male', '2004-06-30');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-student-006', 'UR-00010', 'usr-student-006', 'role-student-003', sysutcdatetime(), 1);
END;

-- 11. Student User 7 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student7')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [student_code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-student-007', 'USR-00011', 'STD-00007', 'student7', 'student7@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-student-007', 'PRF-00011', 'usr-student-007', 'Thi G', 'Do', 'Do Thi G', '0921098765', 'Female', '2004-12-14');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-student-007', 'UR-00011', 'usr-student-007', 'role-student-003', sysutcdatetime(), 1);
END;

-- 12. Staff User 1 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'staff1')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-staff-001', 'USR-00012', 'staff1', 'staff1@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-staff-001', 'PRF-00012', 'usr-staff-001', 'Thi H', 'Bui', 'Bui Thi H', '0910987654', 'Female', '1992-02-28');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-staff-001', 'UR-00012', 'usr-staff-001', 'role-staff-004', sysutcdatetime(), 1);
END;

-- 13. Head User 1 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'head1')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-head-001', 'USR-00013', 'head1', 'head1@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-head-001', 'PRF-00013', 'usr-head-001', 'Quoc I', 'Dang', 'Dr. Dang Quoc I', '0909876543', 'Male', '1978-10-02');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-head-001', 'UR-00013', 'usr-head-001', 'role-head-005', sysutcdatetime(), 1);
END;

-- 14. Advisor User 1 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'advisor1')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-advisor-001', 'USR-00014', 'advisor1', 'advisor1@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-advisor-001', 'PRF-00014', 'usr-advisor-001', 'Van K', 'Ngo', 'Master Ngo Van K', '0898765432', 'Male', '1986-11-19');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-advisor-001', 'UR-00014', 'usr-advisor-001', 'role-advisor-006', sysutcdatetime(), 1);
END;

-- 15. Student User 8 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student8')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [student_code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-student-008', 'USR-00015', 'STD-00008', 'student8', 'student8@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-student-008', 'PRF-00015', 'usr-student-008', 'Van L', 'Truong', 'Truong Van L', '0887654321', 'Male', '2004-03-22');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-student-008', 'UR-00015', 'usr-student-008', 'role-student-003', sysutcdatetime(), 1);
END;

-- 16. Student User 9 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student9')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [student_code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-student-009', 'USR-00016', 'STD-00009', 'student9', 'student9@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-student-009', 'PRF-00016', 'usr-student-009', 'Thi M', 'Dinh', 'Dinh Thi M', '0876543210', 'Female', '2004-07-11');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-student-009', 'UR-00016', 'usr-student-009', 'role-student-003', sysutcdatetime(), 1);
END;

-- 17. Student User 10 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student10')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [student_code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-student-010', 'USR-00017', 'STD-00010', 'student10', 'student10@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-student-010', 'PRF-00017', 'usr-student-010', 'Van N', 'Phung', 'Phung Van N', '0865432109', 'Male', '2004-10-29');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-student-010', 'UR-00017', 'usr-student-010', 'role-student-003', sysutcdatetime(), 1);
END;

-- 18. Student User 11 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student11')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [student_code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-student-011', 'USR-00018', 'STD-00011', 'student11', 'student11@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-student-011', 'PRF-00018', 'usr-student-011', 'Thi P', 'Luu', 'Luu Thi P', '0854321098', 'Female', '2004-05-08');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-student-011', 'UR-00018', 'usr-student-011', 'role-student-003', sysutcdatetime(), 1);
END;

-- 19. Student User 12 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student12')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [student_code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-student-012', 'USR-00019', 'STD-00012', 'student12', 'student12@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-student-012', 'PRF-00019', 'usr-student-012', 'Van Q', 'Duong', 'Duong Van Q', '0843210987', 'Male', '2004-02-17');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-student-012', 'UR-00019', 'usr-student-012', 'role-student-003', sysutcdatetime(), 1);
END;

-- 20. Student User 13 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student13')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [student_code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-student-013', 'USR-00020', 'STD-00013', 'student13', 'student13@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-student-013', 'PRF-00020', 'usr-student-013', 'Thi R', 'Mai', 'Mai Thi R', '0832109876', 'Female', '2004-08-23');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-student-013', 'UR-00020', 'usr-student-013', 'role-student-003', sysutcdatetime(), 1);
END;

-- 21. Student User 14 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'student14')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [student_code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-student-014', 'USR-00021', 'STD-00014', 'student14', 'student14@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-student-014', 'PRF-00021', 'usr-student-014', 'Van S', 'Trieu', 'Trieu Van S', '0821098765', 'Male', '2004-11-04');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-student-014', 'UR-00021', 'usr-student-014', 'role-student-003', sysutcdatetime(), 1);
END;

-- 22. Proctor User 1 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'proctor1')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-proctor-001', 'USR-00022', 'proctor1', 'proctor1@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-proctor-001', 'PRF-00022', 'usr-proctor-001', 'Van T', 'La', 'La Van T', '0810987654', 'Male', '1988-06-12');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-proctor-001', 'UR-00022', 'usr-proctor-001', 'role-proctor-016', sysutcdatetime(), 1);
END;

-- 23. Dorm Manager 1 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'dormmgr1')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-dormmgr-001', 'USR-00023', 'dormmgr1', 'dormmgr1@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-dormmgr-001', 'PRF-00023', 'usr-dormmgr-001', 'Thi U', 'Kieu', 'Kieu Thi U', '0809876543', 'Female', '1984-09-30');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-dormmgr-001', 'UR-00023', 'usr-dormmgr-001', 'role-dormmgr-018', sysutcdatetime(), 1);
END;

-- 24. Accountant User 1 & Profile & Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[users] WHERE [username] = 'accountant1')
BEGIN
    INSERT INTO [dbo].[users] ([id], [code], [username], [email], [password_hash], [provider], [is_active], [is_email_verified])
    VALUES ('usr-accountant-001', 'USR-00024', 'accountant1', 'accountant1@unicore.edu.vn', '$2y$10$qlgpKqrNF4R7SjWw5klB/OGGvOArWISvZiALfIGnt04G.PvwvdCA.', 'system', 1, 1);

    INSERT INTO [dbo].[user_profiles] ([id], [code], [user_id], [first_name], [last_name], [full_name], [phone_number], [gender], [birth_date])
    VALUES ('prof-accountant-001', 'PRF-00024', 'usr-accountant-001', 'Van V', 'Oang', 'Oang Van V', '0798765432', 'Male', '1989-12-05');

    INSERT INTO [dbo].[user_roles] ([id], [code], [user_id], [role_id], [assigned_at], [is_active])
    VALUES ('ur-accountant-001', 'UR-00024', 'usr-accountant-001', 'role-accountant-009', sysutcdatetime(), 1);
END;

-- 25. User Person IDs (CCCD)
IF NOT EXISTS (SELECT 1 FROM [dbo].[user_person_ids] WHERE [user_id] = 'usr-student-001')
BEGIN
    INSERT INTO [dbo].[user_person_ids] ([id], [code], [user_id], [id_number], [full_name], [card_type], [birth_date], [gender], [nationality], [place_of_origin], [place_of_residence], [verification_status])
    VALUES ('upi-student-001', 'UPI-00001', 'usr-student-001', '001203012345', N'TRẦN VĂN A', 'CCCD_CHIP', '2003-08-20', N'Nam', N'Việt Nam', N'Hà Nội', N'Cầu Giấy, Hà Nội', 'VERIFIED');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_person_ids] WHERE [user_id] = 'usr-student-002')
BEGIN
    INSERT INTO [dbo].[user_person_ids] ([id], [code], [user_id], [id_number], [full_name], [card_type], [birth_date], [gender], [nationality], [place_of_origin], [place_of_residence], [verification_status])
    VALUES ('upi-student-002', 'UPI-00002', 'usr-student-002', '001203098765', N'LÊ THỊ B', 'CCCD_CHIP', '2003-11-12', N'Nữ', N'Việt Nam', N'Hải Phòng', N'Nô̂i Bài, Hà Nội', 'VERIFIED');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_person_ids] WHERE [user_id] = 'usr-student-003')
BEGIN
    INSERT INTO [dbo].[user_person_ids] ([id], [code], [user_id], [id_number], [full_name], [card_type], [birth_date], [gender], [nationality], [place_of_origin], [place_of_residence], [verification_status])
    VALUES ('upi-student-003', 'UPI-00003', 'usr-student-003', '001204011223', N'HOÀNG VĂN C', 'CCCD_CHIP', '2004-01-15', N'Nam', N'Việt Nam', N'Đà Nẵng', N'Thanh Khê, Đà Nẵng', 'VERIFIED');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_person_ids] WHERE [user_id] = 'usr-student-004')
BEGIN
    INSERT INTO [dbo].[user_person_ids] ([id], [code], [user_id], [id_number], [full_name], [card_type], [birth_date], [gender], [nationality], [place_of_origin], [place_of_residence], [verification_status])
    VALUES ('upi-student-004', 'UPI-00004', 'usr-student-004', '001204044556', N'PHẠM THỊ D', 'CCCD_CHIP', '2004-04-18', N'Nữ', N'Việt Nam', N'Cần Thơ', N'Ninh Kiều, Cần Thơ', 'VERIFIED');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_person_ids] WHERE [user_id] = 'usr-student-005')
BEGIN
    INSERT INTO [dbo].[user_person_ids] ([id], [code], [user_id], [id_number], [full_name], [card_type], [birth_date], [gender], [nationality], [place_of_origin], [place_of_residence], [verification_status])
    VALUES ('upi-student-005', 'UPI-00005', 'usr-student-005', '001204077889', N'NGUYỄN VĂN E', 'CCCD_CHIP', '2004-09-05', N'Nam', N'Việt Nam', N'Quảng Ninh', N'Hạ Long, Quảng Ninh', 'VERIFIED');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_person_ids] WHERE [user_id] = 'usr-student-006')
BEGIN
    INSERT INTO [dbo].[user_person_ids] ([id], [code], [user_id], [id_number], [full_name], [card_type], [birth_date], [gender], [nationality], [place_of_origin], [place_of_residence], [verification_status])
    VALUES ('upi-student-006', 'UPI-00006', 'usr-student-006', '001204099001', N'VÕ VĂN F', 'CCCD_CHIP', '2004-06-30', N'Nam', N'Việt Nam', N'Nghệ An', N'Vinh, Nghệ An', 'VERIFIED');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_person_ids] WHERE [user_id] = 'usr-student-007')
BEGIN
    INSERT INTO [dbo].[user_person_ids] ([id], [code], [user_id], [id_number], [full_name], [card_type], [birth_date], [gender], [nationality], [place_of_origin], [place_of_residence], [verification_status])
    VALUES ('upi-student-007', 'UPI-00007', 'usr-student-007', '001204022334', N'ĐỖ THỊ G', 'CCCD_CHIP', '2004-12-14', N'Nữ', N'Việt Nam', N'Huế', N'Phú Nhuận, Huế', 'VERIFIED');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_person_ids] WHERE [user_id] = 'usr-student-008')
BEGIN
    INSERT INTO [dbo].[user_person_ids] ([id], [code], [user_id], [id_number], [full_name], [card_type], [birth_date], [gender], [nationality], [place_of_origin], [place_of_residence], [verification_status])
    VALUES ('upi-student-008', 'UPI-00008', 'usr-student-008', '001204055667', N'TRƯƠNG VĂN L', 'CCCD_CHIP', '2004-03-22', N'Nam', N'Việt Nam', N'Nam Định', N'TP. Nam Định', 'VERIFIED');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_person_ids] WHERE [user_id] = 'usr-student-009')
BEGIN
    INSERT INTO [dbo].[user_person_ids] ([id], [code], [user_id], [id_number], [full_name], [card_type], [birth_date], [gender], [nationality], [place_of_origin], [place_of_residence], [verification_status])
    VALUES ('upi-student-009', 'UPI-00009', 'usr-student-009', '001204088990', N'ĐINH THỊ M', 'CCCD_CHIP', '2004-07-11', N'Nữ', N'Việt Nam', N'Thái Bình', N'TP. Thái Bình', 'VERIFIED');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_person_ids] WHERE [user_id] = 'usr-student-010')
BEGIN
    INSERT INTO [dbo].[user_person_ids] ([id], [code], [user_id], [id_number], [full_name], [card_type], [birth_date], [gender], [nationality], [place_of_origin], [place_of_residence], [verification_status])
    VALUES ('upi-student-010', 'UPI-00010', 'usr-student-010', '001204033445', N'PHÙNG VĂN N', 'CCCD_CHIP', '2004-10-29', N'Nam', N'Việt Nam', N'Vĩnh Phúc', N'Vĩnh Yên', 'VERIFIED');
END;

-- 26. MFA Settings & Backup Codes
IF NOT EXISTS (SELECT 1 FROM [dbo].[user_mfa_settings] WHERE [user_id] = 'usr-admin-001')
BEGIN
    INSERT INTO [dbo].[user_mfa_settings] ([id], [code], [user_id], [mfa_method], [secret_key], [is_mfa_enabled], [enabled_at], [is_active])
    VALUES ('mfa-admin-001', 'MFA-00001', 'usr-admin-001', 'TOTP', 'JBSWY3DPEHPK3PXP', 1, sysutcdatetime(), 1);

    INSERT INTO [dbo].[user_mfa_backup_codes] ([id], [user_id], [code_hash], [is_used])
    VALUES ('mbc-admin-001', 'usr-admin-001', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_mfa_settings] WHERE [user_id] = 'usr-lecturer-001')
BEGIN
    INSERT INTO [dbo].[user_mfa_settings] ([id], [code], [user_id], [mfa_method], [secret_key], [is_mfa_enabled], [enabled_at], [is_active])
    VALUES ('mfa-lecturer-001', 'MFA-00002', 'usr-lecturer-001', 'TOTP', 'KRSXG5CTMVRXEZLU', 1, sysutcdatetime(), 1);

    INSERT INTO [dbo].[user_mfa_backup_codes] ([id], [user_id], [code_hash], [is_used])
    VALUES ('mbc-lecturer-001', 'usr-lecturer-001', '9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08', 0);
END;

-- 27. User Tokens
IF NOT EXISTS (SELECT 1 FROM [dbo].[user_tokens] WHERE [user_id] = 'usr-admin-001')
BEGIN
    INSERT INTO [dbo].[user_tokens] ([id], [code], [user_id], [refresh_token], [jti], [ip_address], [user_agent], [issued_at], [expires_at], [is_active])
    VALUES ('tok-admin-001', 'TOK-00001', 'usr-admin-001', 'dGVzdF9yZWZyZXNoX3Rva2VuX2FkbWlu', 'jti-admin-001', '127.0.0.1', 'Mozilla/5.0 Chrome/128.0', sysutcdatetime(), DATEADD(day, 7, sysutcdatetime()), 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_tokens] WHERE [user_id] = 'usr-student-001')
BEGIN
    INSERT INTO [dbo].[user_tokens] ([id], [code], [user_id], [refresh_token], [jti], [ip_address], [user_agent], [issued_at], [expires_at], [is_active])
    VALUES ('tok-student-001', 'TOK-00002', 'usr-student-001', 'dGVzdF9yZWZyZXNoX3Rva2VuX3N0dWRlbnQx', 'jti-student-001', '127.0.0.1', 'Mozilla/5.0 Firefox/129.0', sysutcdatetime(), DATEADD(day, 7, sysutcdatetime()), 1);
END;

-- 28. User External Logins (Google SSO)
IF NOT EXISTS (SELECT 1 FROM [dbo].[user_external_logins] WHERE [user_id] = 'usr-student-001')
BEGIN
    INSERT INTO [dbo].[user_external_logins] ([id], [code], [user_id], [provider], [provider_user_id], [provider_email], [provider_display_name], [is_active])
    VALUES ('ext-student-001', 'EXT-00001', 'usr-student-001', 'Google', 'google-uid-1029384756', 'student1@unicore.edu.vn', 'Tran Van A', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_external_logins] WHERE [user_id] = 'usr-student-003')
BEGIN
    INSERT INTO [dbo].[user_external_logins] ([id], [code], [user_id], [provider], [provider_user_id], [provider_email], [provider_display_name], [is_active])
    VALUES ('ext-student-003', 'EXT-00002', 'usr-student-003', 'Google', 'google-uid-5647382910', 'student3@unicore.edu.vn', 'Hoang Van C', 1);
END;

-- 29. User Auth Logs
IF NOT EXISTS (SELECT 1 FROM [dbo].[user_auth_logs] WHERE [user_id] = 'usr-admin-001')
BEGIN
    INSERT INTO [dbo].[user_auth_logs] ([id], [user_id], [event_type], [ip_address], [user_agent], [description], [created_at])
    VALUES ('ual-admin-001', 'usr-admin-001', 'LOGIN_SUCCESS', '127.0.0.1', 'Mozilla/5.0 Chrome/128.0', 'Admin logged in via Password', sysutcdatetime());
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_auth_logs] WHERE [user_id] = 'usr-student-003')
BEGIN
    INSERT INTO [dbo].[user_auth_logs] ([id], [user_id], [event_type], [ip_address], [user_agent], [description], [created_at])
    VALUES ('ual-student-003', 'usr-student-003', 'LOGIN_SUCCESS', '127.0.0.1', 'Mozilla/5.0 Chrome/128.0', 'Student3 logged in via Google SSO', sysutcdatetime());
END;

GO
