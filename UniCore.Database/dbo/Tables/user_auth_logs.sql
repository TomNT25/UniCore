CREATE TABLE [dbo].[user_auth_logs] (
    [id]          VARCHAR (50)  CONSTRAINT [DF_user_auth_logs_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [user_id]     VARCHAR (50)  NULL,
    [event_type]  VARCHAR (50)  NOT NULL,
    [ip_address]  VARCHAR (45)  NULL,
    [user_agent]  VARCHAR (500) NULL,
    [description] NVARCHAR (500) NULL,
    [created_at]  DATETIME2 (7) DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_user_auth_logs_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[users] ([id]) ON DELETE SET NULL
);
GO

CREATE NONCLUSTERED INDEX [IX_user_auth_logs_user_id_date]
    ON [dbo].[user_auth_logs] ([user_id] ASC, [created_at] DESC);
GO

CREATE NONCLUSTERED INDEX [IX_user_auth_logs_event_type]
    ON [dbo].[user_auth_logs] ([event_type] ASC, [created_at] DESC);
GO

CREATE NONCLUSTERED INDEX [IX_user_auth_logs_created_at]
    ON [dbo].[user_auth_logs] ([created_at] DESC);
GO