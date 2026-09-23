PRINT 'Seeding Whitelisted Emails...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-std1-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std1-email', 'WLE-00001', 'student1@unicore.edu.vn', 1, 'usr-student-001', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-std2-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std2-email', 'WLE-00002', 'student2@unicore.edu.vn', 1, 'usr-student-002', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-std3-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std3-email', 'WLE-00003', 'student3@unicore.edu.vn', 1, 'usr-student-003', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-std4-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std4-email', 'WLE-00004', 'student4@unicore.edu.vn', 1, 'usr-student-004', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-std5-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std5-email', 'WLE-00005', 'student5@unicore.edu.vn', 1, 'usr-student-005', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-std6-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std6-email', 'WLE-00006', 'student6@unicore.edu.vn', 1, 'usr-student-006', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-std7-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std7-email', 'WLE-00007', 'student7@unicore.edu.vn', 1, 'usr-student-007', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-lec2-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-lec2-email', 'WLE-00008', 'lecturer2@unicore.edu.vn', 1, 'usr-lecturer-002', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-lec3-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-lec3-email', 'WLE-00009', 'lecturer3@unicore.edu.vn', 1, 'usr-lecturer-003', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-stf1-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-stf1-email', 'WLE-00010', 'staff1@unicore.edu.vn', 1, 'usr-staff-001', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-head1-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-head1-email', 'WLE-00011', 'head1@unicore.edu.vn', 1, 'usr-head-001', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-adv1-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-adv1-email', 'WLE-00012', 'advisor1@unicore.edu.vn', 1, 'usr-advisor-001', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-std8-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std8-email', 'WLE-00013', 'student8@unicore.edu.vn', 1, 'usr-student-008', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-std9-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std9-email', 'WLE-00014', 'student9@unicore.edu.vn', 1, 'usr-student-009', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-std10-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std10-email', 'WLE-00015', 'student10@unicore.edu.vn', 1, 'usr-student-010', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-std11-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std11-email', 'WLE-00016', 'student11@unicore.edu.vn', 1, 'usr-student-011', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-std12-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std12-email', 'WLE-00017', 'student12@unicore.edu.vn', 1, 'usr-student-012', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-std13-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std13-email', 'WLE-00018', 'student13@unicore.edu.vn', 1, 'usr-student-013', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-std14-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std14-email', 'WLE-00019', 'student14@unicore.edu.vn', 1, 'usr-student-014', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-prc1-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-prc1-email', 'WLE-00020', 'proctor1@unicore.edu.vn', 1, 'usr-proctor-001', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-drm1-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-drm1-email', 'WLE-00021', 'dormmgr1@unicore.edu.vn', 1, 'usr-dormmgr-001', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [id] = 'wle-acc1-email')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-acc1-email', 'WLE-00022', 'accountant1@unicore.edu.vn', 1, 'usr-accountant-001', 1, 0);
END;

GO
