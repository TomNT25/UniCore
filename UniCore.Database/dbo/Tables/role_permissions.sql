CREATE TABLE [dbo].[role_permissions] (
    [id]            VARCHAR (50)  CONSTRAINT [DF_role_permissions_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]          VARCHAR (50)  NULL,
    [role_id]       VARCHAR (50)  NOT NULL,
    [permission_id] VARCHAR (50)  NOT NULL,
    [assigned_at]   DATETIME2 (7) DEFAULT (getdate()) NULL,
    [assigned_by]   INT           NULL,
    [is_active]     BIT           DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_role_permissions_role_permission] UNIQUE NONCLUSTERED ([role_id] ASC, [permission_id] ASC),
    CONSTRAINT [FK_role_permissions_permissions] FOREIGN KEY ([permission_id]) REFERENCES [dbo].[permissions] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_role_permissions_roles] FOREIGN KEY ([role_id]) REFERENCES [dbo].[roles] ([id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_role_permissions_permission_id]
    ON [dbo].[role_permissions] ([permission_id] ASC);
GO
