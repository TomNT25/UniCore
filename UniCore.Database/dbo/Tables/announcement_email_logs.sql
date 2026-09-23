CREATE TABLE [dbo].[announcement_email_logs] (
    [id]              VARCHAR (50)   CONSTRAINT [DF_announcement_email_logs_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [announcement_id] VARCHAR (50)   NOT NULL,
    [student_id]      VARCHAR (50)   NOT NULL,
    [email]           VARCHAR (100)  NOT NULL,
    [status]          VARCHAR (20)   DEFAULT ('PENDING') NOT NULL,
    [sent_at]         DATETIME2 (7)  NULL,
    [error_message]   NVARCHAR (500) NULL,
    [created_at]      DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [CK_announcement_email_logs_status] CHECK ([status] IN ('PENDING', 'SENT', 'FAILED')),
    CONSTRAINT [FK_announcement_email_logs_announcement] FOREIGN KEY ([announcement_id]) REFERENCES [dbo].[announcements] ([id]),
    CONSTRAINT [FK_announcement_email_logs_student] FOREIGN KEY ([student_id]) REFERENCES [dbo].[users] ([id])
);
GO

CREATE NONCLUSTERED INDEX [IX_announcement_email_logs_status]
    ON [dbo].[announcement_email_logs] ([status] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_announcement_email_logs_announcement_id]
    ON [dbo].[announcement_email_logs] ([announcement_id] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_announcement_email_logs_student_id]
    ON [dbo].[announcement_email_logs] ([student_id] ASC);
GO
