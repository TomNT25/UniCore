CREATE TABLE [dbo].[announcement_students] (
    [id]              VARCHAR (50)  CONSTRAINT [DF_announcement_students_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [announcement_id] VARCHAR (50)  NOT NULL,
    [student_id]      VARCHAR (50)  NOT NULL,
    [viewed_at]       DATETIME2 (7) NULL,
    [acknowledged_at] DATETIME2 (7) NULL,
    [is_sent]         BIT           CONSTRAINT [DF_announcement_students_is_sent] DEFAULT ((0)) NOT NULL,
    [created_at]      DATETIME2 (7) DEFAULT (sysutcdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_announcement_students] UNIQUE ([announcement_id], [student_id]),
    CONSTRAINT [FK_announcement_students_announcement] FOREIGN KEY ([announcement_id]) REFERENCES [dbo].[announcements] ([id]),
    CONSTRAINT [FK_announcement_students_student] FOREIGN KEY ([student_id]) REFERENCES [dbo].[users] ([id])
);
GO

CREATE NONCLUSTERED INDEX [IX_announcement_students_student]
    ON [dbo].[announcement_students] ([student_id] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_announcement_students_ack]
    ON [dbo].[announcement_students] ([announcement_id] ASC, [acknowledged_at] ASC);
GO

