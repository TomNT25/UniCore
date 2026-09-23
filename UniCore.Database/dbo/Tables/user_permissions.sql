CREATE TABLE [dbo].[user_permissions] (
    [id]            VARCHAR (50)  CONSTRAINT [DF_user_permissions_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]          VARCHAR (50)  NULL,
    [user_id]       VARCHAR (50)  NOT NULL,
    [permission_id] VARCHAR (50)  NOT NULL,
    [assigned_at]   DATETIME2 (7) DEFAULT (getdate()) NULL,
    [assigned_by]   INT           NULL,
    [is_active]     BIT           DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_user_permissions_user_permission] UNIQUE NONCLUSTERED ([user_id] ASC, [permission_id] ASC),
    CONSTRAINT [FK_user_permissions_permissions] FOREIGN KEY ([permission_id]) REFERENCES [dbo].[permissions] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_user_permissions_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[users] ([id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_user_permissions_permission_id]
    ON [dbo].[user_permissions] ([permission_id] ASC);
GO
