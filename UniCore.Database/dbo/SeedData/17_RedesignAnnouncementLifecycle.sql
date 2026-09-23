-- Redesign announcements per design doc (scopes + lifecycle status + required dates)
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_announcements_scope_type' AND parent_object_id = OBJECT_ID(N'dbo.announcements'))
    ALTER TABLE [dbo].[announcements] DROP CONSTRAINT [CK_announcements_scope_type];
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_announcements_status' AND parent_object_id = OBJECT_ID(N'dbo.announcements'))
    ALTER TABLE [dbo].[announcements] DROP CONSTRAINT [CK_announcements_status];
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_announcements_dates' AND parent_object_id = OBJECT_ID(N'dbo.announcements'))
    ALTER TABLE [dbo].[announcements] DROP CONSTRAINT [CK_announcements_dates];
GO

-- Normalize legacy scopes/status before applying new CHECKs
UPDATE [dbo].[announcements] SET [scope_type] = N'PUBLIC' WHERE [scope_type] IN (N'ALL', N'COHORT');
UPDATE [dbo].[announcements] SET [scope_type] = N'DEPARTMENT' WHERE [scope_type] = N'MAJOR';
UPDATE [dbo].[announcements] SET [scope_type] = N'SPECIFIC_STUDENTS' WHERE [scope_type] = N'STUDENT';
UPDATE [dbo].[announcements]
SET [publish_date] = ISNULL([publish_date], [created_at]),
    [expired_date] = ISNULL([expired_date], DATEADD(day, 30, ISNULL([publish_date], [created_at])));
UPDATE [dbo].[announcements]
SET [expired_date] = DATEADD(day, 1, [publish_date])
WHERE [expired_date] <= [publish_date];
UPDATE [dbo].[announcements]
SET [status] = CASE
    WHEN SYSUTCDATETIME() < [publish_date] THEN N'UPCOMING'
    WHEN SYSUTCDATETIME() >= [expired_date] THEN N'EXPIRED'
    ELSE N'ACTIVE'
END;
GO

ALTER TABLE [dbo].[announcements] ALTER COLUMN [publish_date] DATETIME2 (7) NOT NULL;
ALTER TABLE [dbo].[announcements] ALTER COLUMN [expired_date] DATETIME2 (7) NOT NULL;
GO

ALTER TABLE [dbo].[announcements] WITH CHECK
ADD CONSTRAINT [CK_announcements_scope_type]
CHECK ([scope_type] IN ('PUBLIC', 'STUDENTS', 'DEPARTMENT', 'CLASS', 'COURSE', 'SPECIFIC_STUDENTS'));

ALTER TABLE [dbo].[announcements] WITH CHECK
ADD CONSTRAINT [CK_announcements_status]
CHECK ([status] IN ('UPCOMING', 'ACTIVE', 'EXPIRED'));

ALTER TABLE [dbo].[announcements] WITH CHECK
ADD CONSTRAINT [CK_announcements_dates]
CHECK ([expired_date] > [publish_date]);
GO

IF OBJECT_ID(N'[dbo].[announcement_email_whitelists]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[announcement_email_whitelists] (
        [id]         VARCHAR (50)  CONSTRAINT [DF_announcement_email_whitelists_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
        [type]       VARCHAR (20)  NOT NULL,
        [value]      VARCHAR (255) NOT NULL,
        [status]     VARCHAR (20)  DEFAULT ('ACTIVE') NOT NULL,
        [created_at] DATETIME2 (7) DEFAULT (sysutcdatetime()) NOT NULL,
        [updated_at] DATETIME2 (7) NULL,
        [created_by] VARCHAR (50)  NULL,
        [updated_by] VARCHAR (50)  NULL,
        PRIMARY KEY CLUSTERED ([id] ASC),
        CONSTRAINT [UQ_announcement_email_whitelists] UNIQUE ([type], [value]),
        CONSTRAINT [CK_announcement_email_whitelists_type] CHECK ([type] IN ('EMAIL', 'DOMAIN')),
        CONSTRAINT [CK_announcement_email_whitelists_status] CHECK ([status] IN ('ACTIVE', 'INACTIVE'))
    );
END
GO
