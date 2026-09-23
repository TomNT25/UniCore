IF COL_LENGTH('dbo.announcement_students', 'is_sent') IS NULL
BEGIN
    ALTER TABLE [dbo].[announcement_students]
    ADD [is_sent] BIT NOT NULL CONSTRAINT [DF_announcement_students_is_sent] DEFAULT ((0));
END
GO
