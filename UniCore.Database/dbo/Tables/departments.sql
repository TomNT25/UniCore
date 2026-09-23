CREATE TABLE [dbo].[departments] (
    [id]          VARCHAR (50)   CONSTRAINT [DF_departments_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]        VARCHAR (50)   NULL,
    [name]        NVARCHAR (100) NOT NULL,
    [description] NVARCHAR (255) NULL,
    [is_active]   BIT            DEFAULT ((1)) NOT NULL,
    [is_deleted]  BIT            DEFAULT ((0)) NOT NULL,
    [created_at]  DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [updated_at]  DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [created_by]  VARCHAR (50)   NULL,
    [updated_by]  VARCHAR (50)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_departments_name] UNIQUE ([name]),
    CONSTRAINT [UQ_departments_code] UNIQUE ([code]),
    CONSTRAINT [FK_departments_created_by] FOREIGN KEY ([created_by]) REFERENCES [dbo].[users] ([id]),
    CONSTRAINT [FK_departments_updated_by] FOREIGN KEY ([updated_by]) REFERENCES [dbo].[users] ([id])
);
GO

CREATE NONCLUSTERED INDEX [IX_departments_active_deleted]
    ON [dbo].[departments] ([is_deleted] ASC, [is_active] ASC);
GO
