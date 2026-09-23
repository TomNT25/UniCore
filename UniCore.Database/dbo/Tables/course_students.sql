CREATE TABLE [dbo].[course_students] (
    [id]               VARCHAR (50)   CONSTRAINT [DF_course_students_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]             VARCHAR (50)   NULL,
    [course_id]        VARCHAR (50)   NOT NULL,
    [user_id] VARCHAR (50)   NOT NULL,
    [start_date]       DATETIME2 (7)  NOT NULL,
    [end_date]         DATETIME2 (7)  NOT NULL,
    [weight]           INT            DEFAULT ((1)) NOT NULL,
    [final_score]      NUMERIC (5, 2) NULL,
    [status]           VARCHAR (20)   DEFAULT ('ENROLLED') NULL,
    [is_active]        BIT            DEFAULT ((1)) NOT NULL,
    [is_deleted]       BIT            DEFAULT ((0)) NOT NULL,
    [created_at]       DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [updated_at]       DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [created_by]       VARCHAR (50)   NULL,
    [updated_by]       VARCHAR (50)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_course_students_assignment] UNIQUE ([course_id], [user_id]),
    CONSTRAINT [CK_course_students_weight] CHECK ([weight] > (0)),
    CONSTRAINT [CK_course_students_score] CHECK ([final_score] IS NULL OR ([final_score] >= (0) AND [final_score] <= (10))),
    CONSTRAINT [CK_course_students_dates] CHECK ([end_date] >= [start_date]),
    CONSTRAINT [FK_course_students_courses] FOREIGN KEY ([course_id]) REFERENCES [dbo].[courses] ([id]),
    CONSTRAINT [FK_course_students_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[users] ([id]),
    CONSTRAINT [FK_course_students_created_by] FOREIGN KEY ([created_by]) REFERENCES [dbo].[users] ([id]),
    CONSTRAINT [FK_course_students_updated_by] FOREIGN KEY ([updated_by]) REFERENCES [dbo].[users] ([id])
);
GO

CREATE NONCLUSTERED INDEX [IX_course_students_user_id]
    ON [dbo].[course_students] ([user_id] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_course_students_status]
    ON [dbo].[course_students] ([status] ASC, [is_active] ASC, [is_deleted] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_course_students_code]
    ON [dbo].[course_students] ([code] ASC)
    WHERE [code] IS NOT NULL;
GO
