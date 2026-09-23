CREATE TABLE [dbo].[user_tokens] (
    [id]                VARCHAR (50)  CONSTRAINT [DF_user_tokens_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]              VARCHAR (50)  NULL,
    [user_id]           VARCHAR (50)  NOT NULL,
    [refresh_token]     VARCHAR (MAX) NOT NULL,
    [jti]               VARCHAR (255) NULL,
    [ip_address]        VARCHAR (45)  NULL,
    [user_agent]        VARCHAR (500) NULL,
    [issued_at]         DATETIME2 (7) DEFAULT (getdate()) NULL,
    [expires_at]        DATETIME2 (7) DEFAULT (getdate()) NULL,
    [revoked_at]        DATETIME2 (7) NULL,
    [replaced_by_token] VARCHAR (MAX) NULL,
    [is_active]         BIT           DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_user_tokens_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[users] ([id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_user_tokens_user_id]
    ON [dbo].[user_tokens] ([user_id] ASC, [is_active] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_user_tokens_jti]
    ON [dbo].[user_tokens] ([jti] ASC)
    WHERE [jti] IS NOT NULL;
GO

CREATE NONCLUSTERED INDEX [IX_user_tokens_expires_at]
    ON [dbo].[user_tokens] ([expires_at] ASC);
GO
