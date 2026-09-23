PRINT 'Seeding Roles...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-admin-001')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-admin-001', 'ROL-00001', 'Admin', 'System Administrator with full permissions', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-lecturer-002')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-lecturer-002', 'ROL-00002', 'Lecturer', 'Academic instructor and course supervisor', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-student-003')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-student-003', 'ROL-00003', 'Student', 'Enrolled academic student', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-staff-004')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-staff-004', 'ROL-00004', 'Staff', 'Department academic staff', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-head-005')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-head-005', 'ROL-00005', 'Head of Department', 'Head of academic department and curriculum supervisor', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-advisor-006')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-advisor-006', 'ROL-00006', 'Academic Advisor', 'Advisor for student academic progress and orientation', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-registrar-007')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-registrar-007', 'ROL-00007', 'Registrar', 'Academic registrar manager for grade and enrollment processing', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-ta-008')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-ta-008', 'ROL-00008', 'Teaching Assistant', 'Course teaching assistant and lab monitor', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-accountant-009')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-accountant-009', 'ROL-00009', 'Financial Accountant', 'University tuition and financial controller', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-librarian-010')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-librarian-010', 'ROL-00010', 'Librarian', 'Academic resource and library manager', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-dean-011')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-dean-011', 'ROL-00011', 'Dean', 'Faculty dean overseeing department operations', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-auditor-012')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-auditor-012', 'ROL-00012', 'System Auditor', 'Quality assurance and compliance auditor', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-counselor-013')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-counselor-013', 'ROL-00013', 'Student Counselor', 'Psychological and career guidance counselor', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-eventmgr-014')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-eventmgr-014', 'ROL-00014', 'Event Manager', 'Campus events and extracurricular activities manager', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-researcher-015')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-researcher-015', 'ROL-00015', 'Research Scholar', 'Postdoctoral researcher and lab fellow', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-proctor-016')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-proctor-016', 'ROL-00016', 'Examination Proctor', 'Formal exam hall supervisor and invigilator', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-alumni-017')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-alumni-017', 'ROL-00017', 'Alumni Coordinator', 'University alumni network manager', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-dormmgr-018')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-dormmgr-018', 'ROL-00018', 'Dormitory Supervisor', 'Student dormitory manager and housing lead', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-facility-019')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-facility-019', 'ROL-00019', 'Facility Manager', 'Campus physical infrastructure manager', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-itadmin-020')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-itadmin-020', 'ROL-00020', 'IT Infrastructure Administrator', 'Network administrator and data center manager', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-secofficer-021')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-secofficer-021', 'ROL-00021', 'Campus Security Officer', 'Campus access control and safety officer', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-placement-022')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-placement-022', 'ROL-00022', 'Career Placement Officer', 'Corporate internship and career recruiter', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-health-023')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-health-023', 'ROL-00023', 'Medical Health Officer', 'University infirmary medical practitioner', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [id] = 'role-guest-024')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-guest-024', 'ROL-00024', 'Guest Visitor', 'Temporary external guest account', 1, 0);
END;

GO
