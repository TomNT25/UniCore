PRINT 'Seeding Departments...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [name] = 'Software Engineering')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-se-001', 'DPT-00001', 'Software Engineering', 'Department of Software Engineering and Computing Sciences', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [name] = 'Information Technology')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-it-002', 'DPT-00002', 'Information Technology', 'Department of Information Systems and Networking', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [name] = 'Business Administration')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-ba-003', 'DPT-00003', 'Business Administration', 'Department of International Business and Administration', 1, 0);
END;

GO
