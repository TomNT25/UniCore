CREATE TABLE [dbo].[user_person_ids] (
    [id]                  VARCHAR (50)  CONSTRAINT [DF_user_person_ids_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]                VARCHAR (50)  NULL,
    [user_id]             VARCHAR (50)  NOT NULL,
    [id_number]           VARCHAR (20)  NOT NULL,
    [full_name]           NVARCHAR (200) NOT NULL,
    [card_type]           VARCHAR (30)  DEFAULT ('CCCD_CHIP') NOT NULL,
    [birth_date]          DATE          NULL,
    [gender]              NVARCHAR (10) NULL,
    [nationality]         NVARCHAR (50) DEFAULT (N'Việt Nam') NULL,
    [place_of_origin]     NVARCHAR (300) NULL,
    [place_of_residence]  NVARCHAR (300) NULL,
    [issue_date]          DATE          NULL,
    [expire_date]         DATE          NULL,
    [issue_place]         NVARCHAR (300) NULL,
    [front_image_url]     VARCHAR (MAX) NULL,
    [back_image_url]      VARCHAR (MAX) NULL,
    [verification_status] VARCHAR (30)  DEFAULT ('UNVERIFIED') NOT NULL,
    [verified_at]         DATETIME2 (7) NULL,
    [verified_by]         VARCHAR (50)  NULL,
    [is_active]           BIT           DEFAULT ((1)) NULL,
    [created_at]          DATETIME2 (7) DEFAULT (getdate()) NULL,
    [updated_at]          DATETIME2 (7) DEFAULT (getdate()) NULL,
    [created_by]          VARCHAR (50)  NULL,
    [updated_by]          VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_user_person_ids_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[users] ([id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_user_person_ids_user_id] UNIQUE NONCLUSTERED ([user_id] ASC),
    CONSTRAINT [UQ_user_person_ids_id_number] UNIQUE NONCLUSTERED ([id_number] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_user_person_ids_verification_status]
    ON [dbo].[user_person_ids] ([verification_status] ASC);
GO