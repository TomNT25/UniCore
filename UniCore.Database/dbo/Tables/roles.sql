CREATE TABLE [dbo].[roles] (
    [id]             VARCHAR (50)  CONSTRAINT [DF_roles_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]           VARCHAR (50)  NULL,
    [name]           VARCHAR (50)  NOT NULL,
    [description]    VARCHAR (255) NULL,
    [is_active]      BIT           DEFAULT ((1)) NULL,
    [is_system_role] BIT           DEFAULT ((0)) NULL,
    [is_deleted]     BIT           DEFAULT ((0)) NULL,
    [created_at]     DATETIME2 (7) DEFAULT (getdate()) NULL,
    [updated_at]     DATETIME2 (7) DEFAULT (getdate()) NULL,
    [created_by]     INT           NULL,
    [updated_by]     INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_roles_name] UNIQUE NONCLUSTERED ([name] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_roles_code]
    ON [dbo].[roles] ([code] ASC)
    WHERE [code] IS NOT NULL;
GO
