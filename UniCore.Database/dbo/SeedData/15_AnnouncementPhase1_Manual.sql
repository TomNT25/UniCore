-- Manual migrate/run for announcement scope redesign
-- Scopes: PUBLIC | DEPARTMENT | CLASS | STUDENT

IF EXISTS (
    SELECT 1 FROM sys.check_constraints
    WHERE name = N'CK_announcements_scope_type' AND parent_object_id = OBJECT_ID(N'dbo.announcements')
)
BEGIN
    ALTER TABLE [dbo].[announcements] DROP CONSTRAINT [CK_announcements_scope_type];
END
GO

UPDATE [dbo].[announcements] SET [scope_type] = N'PUBLIC', [scope_value] = NULL WHERE [scope_type] IN (N'ALL', N'COHORT');
UPDATE [dbo].[announcements] SET [scope_type] = N'DEPARTMENT' WHERE [scope_type] = N'MAJOR';
GO

ALTER TABLE [dbo].[announcements] WITH CHECK
ADD CONSTRAINT [CK_announcements_scope_type]
CHECK ([scope_type] IN ('PUBLIC', 'DEPARTMENT', 'CLASS', 'STUDENT'));
GO

IF OBJECT_ID(N'[dbo].[announcement_targets]', N'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[announcement_targets];
END
GO

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
