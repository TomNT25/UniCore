CREATE TABLE [dbo].[courses] (
    [id]            VARCHAR (50)   CONSTRAINT [DF_courses_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]          VARCHAR (50)   NULL,
    [type]          VARCHAR (20)   NOT NULL,
    [name]          NVARCHAR (100) NOT NULL,
    [description]   NVARCHAR (255) NULL,
    [department_id] VARCHAR (50)   NOT NULL,
    [is_active]     BIT            DEFAULT ((1)) NOT NULL,
    [is_deleted]    BIT            DEFAULT ((0)) NOT NULL,
    [created_at]    DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [updated_at]    DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [created_by]    VARCHAR (50)   NULL,
    [updated_by]    VARCHAR (50)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_courses_code] UNIQUE ([code]),
    CONSTRAINT [FK_courses_departments] FOREIGN KEY ([department_id]) REFERENCES [dbo].[departments] ([id]),
    CONSTRAINT [FK_courses_created_by] FOREIGN KEY ([created_by]) REFERENCES [dbo].[users] ([id]),
    CONSTRAINT [FK_courses_updated_by] FOREIGN KEY ([updated_by]) REFERENCES [dbo].[users] ([id])
);
GO

CREATE NONCLUSTERED INDEX [IX_courses_department_id]
    ON [dbo].[courses] ([department_id] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_courses_type]
    ON [dbo].[courses] ([type] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_courses_active_deleted]
    ON [dbo].[courses] ([is_deleted] ASC, [is_active] ASC);
GO
