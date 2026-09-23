PRINT 'Seeding Permissions...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-user-read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-user-read', 'PRM-00001', 'User.Read', 'Read user profiles and lists', 'UserManagement', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-user-write')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-user-write', 'PRM-00002', 'User.Write', 'Create and modify users', 'UserManagement', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-course-read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-course-read', 'PRM-00003', 'Course.Read', 'View academic courses and schedules', 'Academic', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-course-write')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-course-write', 'PRM-00004', 'Course.Write', 'Create and manage academic courses', 'Academic', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-anc-read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-anc-read', 'PRM-00005', 'Announcement.Read', 'View system announcements', 'Communication', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-anc-write')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-anc-write', 'PRM-00006', 'Announcement.Write', 'Create and publish system announcements', 'Communication', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-class-read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-class-read', 'PRM-00007', 'Class.Read', 'View classes and cohort structures', 'Academic', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-class-write')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-class-write', 'PRM-00008', 'Class.Write', 'Create and manage classes and cohorts', 'Academic', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-grade-read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-grade-read', 'PRM-00009', 'Grade.Read', 'View student academic scores and grades', 'Academic', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-grade-write')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-grade-write', 'PRM-00010', 'Grade.Write', 'Input and modify student academic scores', 'Academic', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-log-read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-log-read', 'PRM-00011', 'AuditLog.Read', 'View system operation and security logs', 'System', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-dept-read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-dept-read', 'PRM-00012', 'Department.Read', 'View university departments and faculties', 'Academic', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-dept-write')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-dept-write', 'PRM-00013', 'Department.Write', 'Create and update department information', 'Academic', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-schedule-read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-schedule-read', 'PRM-00014', 'Schedule.Read', 'View timetable and course schedules', 'Academic', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-schedule-write')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-schedule-write', 'PRM-00015', 'Schedule.Write', 'Manage timetable and schedule slots', 'Academic', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-mfa-manage')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-mfa-manage', 'PRM-00016', 'MFA.Manage', 'Manage user multi-factor authentication', 'UserManagement', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-exam-read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-exam-read', 'PRM-00017', 'Examination.Read', 'View examination schedules and rooms', 'Academic', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-exam-write')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-exam-write', 'PRM-00018', 'Examination.Write', 'Create exam slots and assign proctors', 'Academic', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-dorm-read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-dorm-read', 'PRM-00019', 'Dormitory.Read', 'View student dormitory room allocations', 'Facility', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-dorm-write')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-dorm-write', 'PRM-00020', 'Dormitory.Write', 'Manage dormitory rooms and housing beds', 'Facility', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-facility-read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-facility-read', 'PRM-00021', 'Facility.Read', 'View campus halls, rooms and equipment', 'Facility', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-facility-write')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-facility-write', 'PRM-00022', 'Facility.Write', 'Book and manage campus room facilities', 'Facility', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-finance-read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-finance-read', 'PRM-00023', 'Finance.Read', 'View student tuition statements and receipts', 'Finance', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-finance-write')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-finance-write', 'PRM-00024', 'Finance.Write', 'Process tuition payments and fee invoices', 'Finance', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-report-read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-report-read', 'PRM-00025', 'Reports.Read', 'View university analytics and grade reports', 'Analytics', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [id] = 'perm-report-export')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-report-export', 'PRM-00026', 'Reports.Export', 'Export system statistics and report PDFs', 'Analytics', 1, 0);
END;

GO
