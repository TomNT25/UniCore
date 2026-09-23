CREATE TABLE [dbo].[user_roles] (
    [id]            VARCHAR (50)  CONSTRAINT [DF_user_roles_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]          VARCHAR (50)  NULL,
    [user_id]       VARCHAR (50)  NOT NULL,
    [role_id]       VARCHAR (50)  NOT NULL,
    [assigned_at]   DATETIME2 (7) DEFAULT (getdate()) NULL,
    [assigned_by]   INT           NULL,
    [is_active]     BIT           DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_user_roles_user_role] UNIQUE NONCLUSTERED ([user_id] ASC, [role_id] ASC),
    CONSTRAINT [FK_user_roles_roles] FOREIGN KEY ([role_id]) REFERENCES [dbo].[roles] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_user_roles_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[users] ([id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_user_roles_role_id]
    ON [dbo].[user_roles] ([role_id] ASC);
GO