CREATE TABLE [dbo].[user_profiles] (
    [id]                   VARCHAR (50)  CONSTRAINT [DF_user_profiles_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]                 VARCHAR (50)  NULL,
    [user_id]              VARCHAR (50)  NOT NULL,
    [first_name]           NVARCHAR (100) NULL,
    [last_name]            NVARCHAR (100) NULL,
    [full_name]            NVARCHAR (200) NULL,
    [phone_number]         VARCHAR (20)  NULL,
    [avatar_url]           VARCHAR (MAX) NULL,
    [avatar_media_file_id] INT           NULL,
    [gender]               VARCHAR (20)  NULL,
    [birth_date]           DATE          NULL,
    [address]              NVARCHAR (MAX) NULL,
    [bio]                  NVARCHAR (500) NULL,
    [is_active]            BIT           DEFAULT ((1)) NULL,
    [created_at]           DATETIME2 (7) DEFAULT (getdate()) NULL,
    [updated_at]           DATETIME2 (7) DEFAULT (getdate()) NULL,
    [created_by]           VARCHAR (50)  NULL,
    [updated_by]           VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_user_profiles_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[users] ([id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_user_profiles_user_id] UNIQUE NONCLUSTERED ([user_id] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_user_profiles_phone_number]
    ON [dbo].[user_profiles] ([phone_number] ASC)
    WHERE [phone_number] IS NOT NULL;
GO

CREATE NONCLUSTERED INDEX [IX_user_profiles_full_name]
    ON [dbo].[user_profiles] ([full_name] ASC)
    WHERE [full_name] IS NOT NULL;
GO