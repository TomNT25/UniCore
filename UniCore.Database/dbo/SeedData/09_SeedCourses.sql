PRINT 'Seeding Courses...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [name] = 'Data Structures and Algorithms')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-dsa-001', 'CRS-00001', 'REQUIRED', 'Data Structures and Algorithms', 'Fundamental concepts of linear & non-linear data structures', 'dpt-se-001', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [name] = 'Database Management Systems')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-dbms-002', 'CRS-00002', 'REQUIRED', 'Database Management Systems', 'Relational database design, 3NF normalization, SQL & EF Core', 'dpt-se-001', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [name] = 'Web Application Development')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-web-003', 'CRS-00003', 'ELECTIVE', 'Web Application Development', 'Building scalable web applications with ASP.NET Core & React', 'dpt-it-002', 1, 0);
END;

GO
