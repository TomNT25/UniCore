CREATE TABLE [dbo].[user_external_logins] (
    [id]                   VARCHAR (50)  CONSTRAINT [DF_user_external_logins_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]                 VARCHAR (50)  NULL,
    [user_id]              VARCHAR (50)  NOT NULL,
    [provider]             VARCHAR (50)  NOT NULL,
    [provider_user_id]     VARCHAR (255) NOT NULL,
    [provider_email]       VARCHAR (100) NULL,
    [provider_display_name] NVARCHAR (200) NULL,
    [avatar_url]           VARCHAR (MAX) NULL,
    [access_token]         VARCHAR (MAX) NULL,
    [refresh_token]        VARCHAR (MAX) NULL,
    [token_expires_at]     DATETIME2 (7) NULL,
    [is_active]            BIT           DEFAULT ((1)) NULL,
    [created_at]           DATETIME2 (7) DEFAULT (getdate()) NULL,
    [updated_at]           DATETIME2 (7) DEFAULT (getdate()) NULL,
    [created_by]           VARCHAR (50)  NULL,
    [updated_by]           VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_user_external_logins_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[users] ([id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_user_external_logins_provider_key] UNIQUE NONCLUSTERED ([provider] ASC, [provider_user_id] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_user_external_logins_user_id]
    ON [dbo].[user_external_logins] ([user_id] ASC);
GO