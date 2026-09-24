IF COL_LENGTH('dbo.user_profiles', 'is_verified') IS NULL
BEGIN
    ALTER TABLE [dbo].[user_profiles]
    ADD [is_verified] BIT NOT NULL CONSTRAINT [DF_user_profiles_is_verified] DEFAULT ((0));
END
GO

IF COL_LENGTH('dbo.user_profiles', 'verified_at') IS NULL
BEGIN
    ALTER TABLE [dbo].[user_profiles]
    ADD [verified_at] DATETIME2 (7) NULL;
END
GO

IF COL_LENGTH('dbo.user_profiles', 'verified_by') IS NULL
BEGIN
    ALTER TABLE [dbo].[user_profiles]
    ADD [verified_by] VARCHAR (50) NULL;
END
GO
