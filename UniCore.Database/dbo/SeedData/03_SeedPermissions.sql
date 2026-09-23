PRINT 'Seeding Permissions...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [name] = 'User.Read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-user-read', 'PRM-00001', 'User.Read', 'Read user profiles and lists', 'UserManagement', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [name] = 'User.Write')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-user-write', 'PRM-00002', 'User.Write', 'Create and modify users', 'UserManagement', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [name] = 'Course.Read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-course-read', 'PRM-00003', 'Course.Read', 'View academic courses and schedules', 'Academic', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [name] = 'Course.Write')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-course-write', 'PRM-00004', 'Course.Write', 'Create and manage academic courses', 'Academic', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [name] = 'Announcement.Read')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-anc-read', 'PRM-00005', 'Announcement.Read', 'View system announcements', 'Communication', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [name] = 'Announcement.Write')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-anc-write', 'PRM-00006', 'Announcement.Write', 'Create and edit draft announcements', 'Communication', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [name] = 'Announcement.Publish')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-anc-publish', 'PRM-00007', 'Announcement.Publish', 'Publish or cancel announcements', 'Communication', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[permissions] WHERE [name] = 'Announcement.Report')
BEGIN
    INSERT INTO [dbo].[permissions] ([id], [code], [name], [description], [resource], [is_active], [is_deleted])
    VALUES ('perm-anc-report', 'PRM-00008', 'Announcement.Report', 'View announcement delivery reports', 'Communication', 1, 0);
END;

GO
