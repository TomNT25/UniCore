CREATE TABLE [dbo].[user_mfa_settings] (
    [id]             VARCHAR (50)  CONSTRAINT [DF_user_mfa_settings_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]           VARCHAR (50)  NULL,
    [user_id]        VARCHAR (50)  NOT NULL,
    [mfa_method]     VARCHAR (30)  DEFAULT ('TOTP') NOT NULL,
    [secret_key]     VARCHAR (255) NULL,
    [is_mfa_enabled] BIT           DEFAULT ((0)) NOT NULL,
    [enabled_at]     DATETIME2 (7) NULL,
    [is_active]      BIT           DEFAULT ((1)) NULL,
    [created_at]     DATETIME2 (7) DEFAULT (getdate()) NULL,
    [updated_at]     DATETIME2 (7) DEFAULT (getdate()) NULL,
    [created_by]     VARCHAR (50)  NULL,
    [updated_by]     VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_user_mfa_settings_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[users] ([id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_user_mfa_settings_user_id] UNIQUE ([user_id])
);