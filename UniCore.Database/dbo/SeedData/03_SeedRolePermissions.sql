PRINT 'Seeding Role Permissions...';

-- Admin Role Permissions
IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-admin-001' AND [permission_id] = 'perm-user-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-admin-ur', 'RPM-00001', 'role-admin-001', 'perm-user-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-admin-001' AND [permission_id] = 'perm-user-write')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-admin-uw', 'RPM-00002', 'role-admin-001', 'perm-user-write', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-admin-001' AND [permission_id] = 'perm-course-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-admin-cr', 'RPM-00003', 'role-admin-001', 'perm-course-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-admin-001' AND [permission_id] = 'perm-course-write')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-admin-cw', 'RPM-00004', 'role-admin-001', 'perm-course-write', 1);

-- Lecturer Role Permissions
IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-lecturer-002' AND [permission_id] = 'perm-course-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-lec-cr', 'RPM-00005', 'role-lecturer-002', 'perm-course-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-lecturer-002' AND [permission_id] = 'perm-course-write')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-lec-cw', 'RPM-00006', 'role-lecturer-002', 'perm-course-write', 1);

-- Student Role Permissions
IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-student-003' AND [permission_id] = 'perm-course-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-std-cr', 'RPM-00007', 'role-student-003', 'perm-course-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-student-003' AND [permission_id] = 'perm-anc-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-std-ar', 'RPM-00008', 'role-student-003', 'perm-anc-read', 1);

-- Additional Role Permissions
IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-admin-001' AND [permission_id] = 'perm-anc-write')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-admin-aw', 'RPM-00009', 'role-admin-001', 'perm-anc-write', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-admin-001' AND [permission_id] = 'perm-log-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-admin-lr', 'RPM-00010', 'role-admin-001', 'perm-log-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-head-005' AND [permission_id] = 'perm-course-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-head-cr', 'RPM-00011', 'role-head-005', 'perm-course-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-head-005' AND [permission_id] = 'perm-course-write')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-head-cw', 'RPM-00012', 'role-head-005', 'perm-course-write', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-head-005' AND [permission_id] = 'perm-dept-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-head-dr', 'RPM-00013', 'role-head-005', 'perm-dept-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-advisor-006' AND [permission_id] = 'perm-user-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-adv-ur', 'RPM-00014', 'role-advisor-006', 'perm-user-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-advisor-006' AND [permission_id] = 'perm-grade-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-adv-gr', 'RPM-00015', 'role-advisor-006', 'perm-grade-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-registrar-007' AND [permission_id] = 'perm-class-write')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-reg-cw', 'RPM-00016', 'role-registrar-007', 'perm-class-write', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-registrar-007' AND [permission_id] = 'perm-grade-write')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-reg-gw', 'RPM-00017', 'role-registrar-007', 'perm-grade-write', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-ta-008' AND [permission_id] = 'perm-grade-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-ta-gr', 'RPM-00018', 'role-ta-008', 'perm-grade-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-ta-008' AND [permission_id] = 'perm-schedule-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-ta-sr', 'RPM-00019', 'role-ta-008', 'perm-schedule-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-dean-011' AND [permission_id] = 'perm-dept-write')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-dean-dw', 'RPM-00020', 'role-dean-011', 'perm-dept-write', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-proctor-016' AND [permission_id] = 'perm-exam-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-prc-er', 'RPM-00021', 'role-proctor-016', 'perm-exam-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-proctor-016' AND [permission_id] = 'perm-exam-write')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-prc-ew', 'RPM-00022', 'role-proctor-016', 'perm-exam-write', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-dormmgr-018' AND [permission_id] = 'perm-dorm-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-drm-dr', 'RPM-00023', 'role-dormmgr-018', 'perm-dorm-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-dormmgr-018' AND [permission_id] = 'perm-dorm-write')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-drm-dw', 'RPM-00024', 'role-dormmgr-018', 'perm-dorm-write', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-facility-019' AND [permission_id] = 'perm-facility-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-fac-fr', 'RPM-00025', 'role-facility-019', 'perm-facility-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-facility-019' AND [permission_id] = 'perm-facility-write')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-fac-fw', 'RPM-00026', 'role-facility-019', 'perm-facility-write', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-accountant-009' AND [permission_id] = 'perm-finance-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-acc-fr', 'RPM-00027', 'role-accountant-009', 'perm-finance-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-accountant-009' AND [permission_id] = 'perm-finance-write')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-acc-fw', 'RPM-00028', 'role-accountant-009', 'perm-finance-write', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-auditor-012' AND [permission_id] = 'perm-report-read')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-aud-rr', 'RPM-00029', 'role-auditor-012', 'perm-report-read', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[role_permissions] WHERE [role_id] = 'role-auditor-012' AND [permission_id] = 'perm-report-export')
    INSERT INTO [dbo].[role_permissions] ([id], [code], [role_id], [permission_id], [is_active]) VALUES ('rp-aud-re', 'RPM-00030', 'role-auditor-012', 'perm-report-export', 1);

GO
