-- Expand scope_value for multi-target JSON (DEPARTMENT / CLASS / COURSE).
IF COL_LENGTH('dbo.announcements', 'scope_value') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[announcements]
    ALTER COLUMN [scope_value] VARCHAR (4000) NULL;
END
GO
