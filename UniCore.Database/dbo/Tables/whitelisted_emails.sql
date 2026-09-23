CREATE TABLE [dbo].[whitelisted_emails] (
    [id]           VARCHAR (50)  CONSTRAINT [DF_whitelisted_emails_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]         VARCHAR (50)  NULL,
    [email]        VARCHAR (100) NOT NULL,
    [is_confirmed] BIT           DEFAULT ((0)) NOT NULL,
    [student_id]   VARCHAR (50)  NOT NULL,
    [is_active]    BIT           DEFAULT ((1)) NOT NULL,
    [is_deleted]   BIT           DEFAULT ((0)) NOT NULL,
    [created_at]   DATETIME2 (7) DEFAULT (getdate()) NOT NULL,
    [updated_at]   DATETIME2 (7) DEFAULT (getdate()) NOT NULL,
    [created_by]   VARCHAR (50)  NULL,
    [updated_by]   VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_whitelisted_emails_email] UNIQUE ([email]),
    CONSTRAINT [FK_whitelisted_emails_users] FOREIGN KEY ([student_id]) REFERENCES [dbo].[users] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_whitelisted_emails_created_by] FOREIGN KEY ([created_by]) REFERENCES [dbo].[users] ([id]),
    CONSTRAINT [FK_whitelisted_emails_updated_by] FOREIGN KEY ([updated_by]) REFERENCES [dbo].[users] ([id])
);
GO

CREATE NONCLUSTERED INDEX [IX_whitelisted_emails_student_id]
    ON [dbo].[whitelisted_emails] ([student_id] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_whitelisted_emails_confirmed_active]
    ON [dbo].[whitelisted_emails] ([is_confirmed] ASC, [is_active] ASC, [is_deleted] ASC);
GO