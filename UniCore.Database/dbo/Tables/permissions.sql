CREATE TABLE [dbo].[permissions] (
    [id]          VARCHAR (50)  CONSTRAINT [DF_permissions_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]        VARCHAR (50)  NULL,
    [name]        VARCHAR (100) NOT NULL,
    [description] VARCHAR (255) NULL,
    [resource]    VARCHAR (50)  NOT NULL,
    [action]      VARCHAR (50)  NULL,
    [is_active]   BIT           DEFAULT ((1)) NULL,
    [is_deleted]  BIT           DEFAULT ((0)) NULL,
    [created_at]  DATETIME2 (7) DEFAULT (getdate()) NULL,
    [updated_at]  DATETIME2 (7) DEFAULT (getdate()) NULL,
    [created_by]  INT           NULL,
    [updated_by]  INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_permissions_name] UNIQUE NONCLUSTERED ([name] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_permissions_resource_action]
    ON [dbo].[permissions] ([resource] ASC, [action] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_permissions_code]
    ON [dbo].[permissions] ([code] ASC)
    WHERE [code] IS NOT NULL;
GO
