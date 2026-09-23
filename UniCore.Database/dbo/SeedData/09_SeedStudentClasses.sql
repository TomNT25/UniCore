PRINT 'Seeding Student Classes...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student1-se1701')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student1-se1701', 'SLC-00001', 'cls-se1701', 'usr-student-001', '2026-09-01', '2026-12-31', 8.50, 'Dr. Nguyen Vinh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student2-se1701')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student2-se1701', 'SLC-00002', 'cls-se1701', 'usr-student-002', '2026-09-01', '2026-12-31', 9.20, 'Dr. Nguyen Vinh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student3-ds1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student3-ds1801', 'SLC-00003', 'cls-ds1801', 'usr-student-003', '2026-09-01', '2026-12-31', 8.80, 'Dr. Le Hoang', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student4-ai1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student4-ai1801', 'SLC-00004', 'cls-ai1801', 'usr-student-004', '2026-09-01', '2026-12-31', 9.10, 'Dr. Pham Minh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student5-cy1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student5-cy1801', 'SLC-00005', 'cls-cy1801', 'usr-student-005', '2026-09-01', '2026-12-31', 8.30, 'Dr. Nguyen Vinh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student6-gd1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student6-gd1801', 'SLC-00006', 'cls-gd1801', 'usr-student-006', '2026-09-01', '2026-12-31', 8.75, 'Dr. Le Hoang', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student7-ba1701')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student7-ba1701', 'SLC-00007', 'cls-ba1701', 'usr-student-007', '2026-09-01', '2026-12-31', 9.00, 'Dr. Pham Minh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student3-se1702')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student3-se1702', 'SLC-00008', 'cls-se1702', 'usr-student-003', '2026-09-01', '2026-12-31', 8.40, 'Dr. Nguyen Vinh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student4-it1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student4-it1801', 'SLC-00009', 'cls-it1801', 'usr-student-004', '2026-09-01', '2026-12-31', 8.95, 'Dr. Le Hoang', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student5-fin1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student5-fin1801', 'SLC-00010', 'cls-fin1801', 'usr-student-005', '2026-09-01', '2026-12-31', 9.30, 'Dr. Pham Minh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student6-mkt1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student6-mkt1801', 'SLC-00011', 'cls-mkt1801', 'usr-student-006', '2026-09-01', '2026-12-31', 8.60, 'Dr. Nguyen Vinh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student7-cs1701')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student7-cs1701', 'SLC-00012', 'cls-cs1701', 'usr-student-007', '2026-09-01', '2026-12-31', 9.40, 'Dr. Le Hoang', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student8-bio1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student8-bio1801', 'SLC-00013', 'cls-bio1801', 'usr-student-008', '2026-09-01', '2026-12-31', 8.70, 'Dr. Nguyen Vinh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student9-me1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student9-me1801', 'SLC-00014', 'cls-me1801', 'usr-student-009', '2026-09-01', '2026-12-31', 9.15, 'Dr. Le Hoang', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student10-ce1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student10-ce1801', 'SLC-00015', 'cls-ce1801', 'usr-student-010', '2026-09-01', '2026-12-31', 8.55, 'Dr. Pham Minh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student11-chem1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student11-chem1801', 'SLC-00016', 'cls-chem1801', 'usr-student-011', '2026-09-01', '2026-12-31', 9.05, 'Dr. Nguyen Vinh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student12-arch1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student12-arch1801', 'SLC-00017', 'cls-arch1801', 'usr-student-012', '2026-09-01', '2026-12-31', 8.85, 'Dr. Le Hoang', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student13-env1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student13-env1801', 'SLC-00018', 'cls-env1801', 'usr-student-013', '2026-09-01', '2026-12-31', 8.90, 'Dr. Pham Minh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student14-psy1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student14-psy1801', 'SLC-00019', 'cls-psy1801', 'usr-student-014', '2026-09-01', '2026-12-31', 9.25, 'Dr. Nguyen Vinh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student8-ee1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student8-ee1801', 'SLC-00020', 'cls-ee1801', 'usr-student-008', '2026-09-01', '2026-12-31', 8.65, 'Dr. Le Hoang', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student9-eng1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student9-eng1801', 'SLC-00021', 'cls-eng1801', 'usr-student-009', '2026-09-01', '2026-12-31', 9.45, 'Dr. Pham Minh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [id] = 'slc-student14-logistics1801')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student14-logistics1801', 'SLC-00022', 'cls-logistics1801', 'usr-student-014', '2026-09-01', '2026-12-31', 8.95, 'Dr. Nguyen Vinh', 'ACTIVE', 1, 0);
END;

GO
