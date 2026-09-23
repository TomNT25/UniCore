CREATE TABLE [dbo].[announcements] (
    [id]                      VARCHAR (50)   CONSTRAINT [DF_announcements_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]                    VARCHAR (50)   NULL,
    [title]                   NVARCHAR (255) NOT NULL,
    [description]             NVARCHAR (500) NULL,
    [content]                 NVARCHAR (MAX) NOT NULL,
    [type]                    VARCHAR (20)   NOT NULL,
    [status]                  VARCHAR (20)   DEFAULT ('DRAFT') NOT NULL,
    [scope_type]              VARCHAR (20)   NOT NULL,
    [scope_value]             VARCHAR (50)   NULL,
    [require_acknowledgement] BIT            DEFAULT ((0)) NOT NULL,
    [publish_date]            DATETIME2 (7)  NULL,
    [expired_date]            DATETIME2 (7)  NULL,
    [recipient_count]         INT            NULL,
    [created_at]              DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    [updated_at]              DATETIME2 (7)  NULL,
    [created_by]              VARCHAR (50)   NULL,
    [updated_by]              VARCHAR (50)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [CK_announcements_type] CHECK ([type] IN ('NORMAL', 'IMPORTANT', 'URGENT')),
    CONSTRAINT [CK_announcements_status] CHECK ([status] IN ('DRAFT', 'PUBLISHED', 'CANCELLED')),
    CONSTRAINT [CK_announcements_scope_type] CHECK ([scope_type] IN ('ALL', 'COHORT', 'CLASS', 'MAJOR', 'STUDENT')),
    CONSTRAINT [CK_announcements_dates] CHECK ([expired_date] IS NULL OR [publish_date] IS NULL OR [expired_date] > [publish_date]),
    CONSTRAINT [FK_announcements_created_by] FOREIGN KEY ([created_by]) REFERENCES [dbo].[users] ([id]),
    CONSTRAINT [FK_announcements_updated_by] FOREIGN KEY ([updated_by]) REFERENCES [dbo].[users] ([id])
);
GO

-- CREATE NONCLUSTERED INDEX [IX_announcements_status_publish_date]
   -- ON [dbo].[announcements] ([status] ASC, [publish_date] ASC);
--GO

CREATE NONCLUSTERED INDEX [IX_announcements_scope]
    ON [dbo].[announcements] ([scope_type] ASC, [scope_value] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_announcements_code]
    ON [dbo].[announcements] ([code] ASC)
    WHERE [code] IS NOT NULL;
GO

CREATE NONCLUSTERED INDEX [IX_announcements_created_by]
    ON [dbo].[announcements] ([created_by] ASC);
GO