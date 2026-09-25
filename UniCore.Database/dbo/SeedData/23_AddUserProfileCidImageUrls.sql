IF COL_LENGTH('dbo.user_profiles', 'cid_front_image_url') IS NULL
BEGIN
    ALTER TABLE [dbo].[user_profiles]
    ADD [cid_front_image_url] VARCHAR (MAX) NULL;
END
GO

IF COL_LENGTH('dbo.user_profiles', 'cid_back_image_url') IS NULL
BEGIN
    ALTER TABLE [dbo].[user_profiles]
    ADD [cid_back_image_url] VARCHAR (MAX) NULL;
END
GO
