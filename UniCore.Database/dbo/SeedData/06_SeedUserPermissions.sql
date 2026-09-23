PRINT 'Seeding User Direct Permissions...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-lecturer-001' AND [permission_id] = 'perm-user-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-lec1-ur', 'UPM-00001', 'usr-lecturer-001', 'perm-user-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-lecturer-002' AND [permission_id] = 'perm-course-write')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-lec2-cw', 'UPM-00002', 'usr-lecturer-002', 'perm-course-write', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-lecturer-003' AND [permission_id] = 'perm-course-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-lec3-cr', 'UPM-00003', 'usr-lecturer-003', 'perm-course-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-staff-001' AND [permission_id] = 'perm-user-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-stf1-ur', 'UPM-00004', 'usr-staff-001', 'perm-user-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-student-003' AND [permission_id] = 'perm-course-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-std3-cr', 'UPM-00005', 'usr-student-003', 'perm-course-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-student-004' AND [permission_id] = 'perm-anc-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-std4-ar', 'UPM-00006', 'usr-student-004', 'perm-anc-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-student-005' AND [permission_id] = 'perm-course-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-std5-cr', 'UPM-00007', 'usr-student-005', 'perm-course-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-head-001' AND [permission_id] = 'perm-dept-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-head1-dr', 'UPM-00008', 'usr-head-001', 'perm-dept-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-advisor-001' AND [permission_id] = 'perm-grade-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-adv1-gr', 'UPM-00009', 'usr-advisor-001', 'perm-grade-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-student-006' AND [permission_id] = 'perm-anc-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-std6-ar', 'UPM-00010', 'usr-student-006', 'perm-anc-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-student-007' AND [permission_id] = 'perm-course-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-std7-cr', 'UPM-00011', 'usr-student-007', 'perm-course-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-staff-001' AND [permission_id] = 'perm-class-write')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-stf1-cw', 'UPM-00012', 'usr-staff-001', 'perm-class-write', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-student-008' AND [permission_id] = 'perm-course-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-std8-cr', 'UPM-00013', 'usr-student-008', 'perm-course-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-student-009' AND [permission_id] = 'perm-anc-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-std9-ar', 'UPM-00014', 'usr-student-009', 'perm-anc-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-student-010' AND [permission_id] = 'perm-course-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-std10-cr', 'UPM-00015', 'usr-student-010', 'perm-course-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-student-011' AND [permission_id] = 'perm-anc-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-std11-ar', 'UPM-00016', 'usr-student-011', 'perm-anc-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-student-012' AND [permission_id] = 'perm-course-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-std12-cr', 'UPM-00017', 'usr-student-012', 'perm-course-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-proctor-001' AND [permission_id] = 'perm-exam-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-prc1-er', 'UPM-00018', 'usr-proctor-001', 'perm-exam-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-dormmgr-001' AND [permission_id] = 'perm-dorm-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-drm1-dr', 'UPM-00019', 'usr-dormmgr-001', 'perm-dorm-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-accountant-001' AND [permission_id] = 'perm-finance-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-acc1-fr', 'UPM-00020', 'usr-accountant-001', 'perm-finance-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-student-013' AND [permission_id] = 'perm-anc-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-std13-ar', 'UPM-00021', 'usr-student-013', 'perm-anc-read', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[user_permissions] WHERE [user_id] = 'usr-student-014' AND [permission_id] = 'perm-course-read')
BEGIN
    INSERT INTO [dbo].[user_permissions] ([id], [code], [user_id], [permission_id], [is_active])
    VALUES ('up-std14-cr', 'UPM-00022', 'usr-student-014', 'perm-course-read', 1);
END;

GO
