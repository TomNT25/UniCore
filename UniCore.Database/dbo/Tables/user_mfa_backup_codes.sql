CREATE TABLE [dbo].[user_mfa_backup_codes] (
    [id]         VARCHAR (50)  CONSTRAINT [DF_user_mfa_backup_codes_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [user_id]    VARCHAR (50)  NOT NULL,
    [code_hash]  VARCHAR (255) NOT NULL,
    [is_used]    BIT           DEFAULT ((0)) NOT NULL,
    [used_at]    DATETIME2 (7) NULL,
    [created_at] DATETIME2 (7) DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_user_mfa_backup_codes_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[users] ([id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_user_mfa_backup_codes_user_id]
    ON [dbo].[user_mfa_backup_codes] ([user_id] ASC, [is_used] ASC)
    INCLUDE ([code_hash]);
GO