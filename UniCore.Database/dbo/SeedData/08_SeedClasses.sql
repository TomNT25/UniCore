PRINT 'Seeding School Classes...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-se1701')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-se1701', 'CLS-00001', 'SE1701', 'Software Engineering Class Cohort 17 - Room 302', 'dpt-se-001', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-se1702')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-se1702', 'CLS-00002', 'SE1702', 'Software Engineering Class Cohort 17 - Room 304', 'dpt-se-001', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-it1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-it1801', 'CLS-00003', 'IT1801', 'Information Technology Class Cohort 18 - Room 401', 'dpt-it-002', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-ds1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-ds1801', 'CLS-00004', 'DS1801', 'Data Science Class Cohort 18 - Room 305', 'dpt-ds-004', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-ai1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-ai1801', 'CLS-00005', 'AI1801', 'Artificial Intelligence Cohort 18 - Room 308', 'dpt-ai-005', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-cy1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-cy1801', 'CLS-00006', 'CY1801', 'Cyber Security Cohort 18 - Room 402', 'dpt-cy-006', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-gd1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-gd1801', 'CLS-00007', 'GD1801', 'Graphic Design Cohort 18 - Studio 101', 'dpt-gd-007', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-ba1701')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-ba1701', 'CLS-00008', 'BA1701', 'Business Administration Cohort 17 - Room 201', 'dpt-ba-003', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-fin1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-fin1801', 'CLS-00009', 'FIN1801', 'Finance & Banking Cohort 18 - Room 205', 'dpt-fin-008', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-mkt1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-mkt1801', 'CLS-00010', 'MKT1801', 'Digital Marketing Cohort 18 - Room 207', 'dpt-mkt-009', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-cs1701')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-cs1701', 'CLS-00011', 'CS1701', 'Computer Science Cohort 17 - Room 310', 'dpt-cs-010', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-ee1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-ee1801', 'CLS-00012', 'EE1801', 'Electrical Engineering Cohort 18 - Lab 501', 'dpt-ee-011', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-eng1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-eng1801', 'CLS-00013', 'ENG1801', 'Business English Cohort 18 - Room 102', 'dpt-eng-012', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-law1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-law1801', 'CLS-00014', 'LAW1801', 'Cyber Law Cohort 18 - Room 104', 'dpt-law-013', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-bio1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-bio1801', 'CLS-00015', 'BIO1801', 'Biotechnology Cohort 18 - Lab 201', 'dpt-bio-015', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-me1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-me1801', 'CLS-00016', 'ME1801', 'Mechanical Engineering Cohort 18 - Workshop 1', 'dpt-me-016', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-ce1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-ce1801', 'CLS-00017', 'CE1801', 'Civil Engineering Cohort 18 - Room 301', 'dpt-ce-017', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-chem1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-chem1801', 'CLS-00018', 'CHEM1801', 'Chemical Engineering Cohort 18 - Lab 303', 'dpt-chem-018', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-arch1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-arch1801', 'CLS-00019', 'ARCH1801', 'Architecture Cohort 18 - Studio 202', 'dpt-arch-019', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-env1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-env1801', 'CLS-00020', 'ENV1801', 'Environmental Science Cohort 18 - Room 105', 'dpt-env-020', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-psy1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-psy1801', 'CLS-00021', 'PSY1801', 'Applied Psychology Cohort 18 - Room 107', 'dpt-psy-021', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-media1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-media1801', 'CLS-00022', 'MEDIA1801', 'Mass Media Cohort 18 - Studio 301', 'dpt-media-022', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-hotel1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-hotel1801', 'CLS-00023', 'HOTEL1801', 'Tourism & Hospitality Cohort 18 - Hall B', 'dpt-hotel-023', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [id] = 'cls-logistics1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('cls-logistics1801', 'CLS-00024', 'LOGISTICS1801', 'Supply Chain Cohort 18 - Room 209', 'dpt-logistics-024', 1, 0);
END;

GO
