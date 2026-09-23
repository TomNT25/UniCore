CREATE TABLE [dbo].[users] (
    [id]                    VARCHAR (50)  CONSTRAINT [DF_users_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]                  VARCHAR (50)  NULL,
    [student_code]          VARCHAR (50)  NULL,
    [class_id]              VARCHAR (50)  NULL,
    [username]              VARCHAR (50)  NOT NULL,
    [email]                 VARCHAR (100) NOT NULL,
    [password_hash]         VARCHAR (255) NOT NULL,
    [provider]              VARCHAR (50)  DEFAULT ('system') NULL,
    [failed_login_attempts] INT           DEFAULT ((0)) NOT NULL,
    [lockout_end]           DATETIME2 (7) NULL,
    [is_active]             BIT           DEFAULT ((1)) NULL,
    [is_email_verified]     BIT           DEFAULT ((0)) NULL,
    [email_verified_at]     DATETIME2 (7) NULL,
    [last_login_at]         DATETIME2 (7) NULL,
    [created_at]            DATETIME2 (7) DEFAULT (getdate()) NULL,
    [updated_at]            DATETIME2 (7) DEFAULT (getdate()) NULL,
    [created_by]            VARCHAR (50)  NULL,
    [updated_by]            VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_users_classes] FOREIGN KEY ([class_id]) REFERENCES [dbo].[classes] ([id]),
    CONSTRAINT [UQ_users_email] UNIQUE NONCLUSTERED ([email] ASC),
    CONSTRAINT [UQ_users_username] UNIQUE NONCLUSTERED ([username] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_users_class_id]
    ON [dbo].[users] ([class_id] ASC)
    WHERE [class_id] IS NOT NULL;
GO

CREATE NONCLUSTERED INDEX [IX_users_student_code]
    ON [dbo].[users] ([student_code] ASC)
    WHERE [student_code] IS NOT NULL;
GO

CREATE NONCLUSTERED INDEX [IX_users_code]
    ON [dbo].[users] ([code] ASC)
    WHERE [code] IS NOT NULL;
GO

CREATE NONCLUSTERED INDEX [IX_users_is_active]
    ON [dbo].[users] ([is_active] ASC);
GO