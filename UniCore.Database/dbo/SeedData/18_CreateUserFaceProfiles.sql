-- Migration: Create user_face_profiles table for face authentication
-- Phase 2 of Face Recognition Auth feature

-- Check if table already exists
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'user_face_profiles' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[user_face_profiles]
    (
        [id]                    NVARCHAR(50)    NOT NULL    DEFAULT dbo.fn_GenerateUUIDv7(),
        [user_id]               NVARCHAR(50)    NOT NULL,
        [status]                NVARCHAR(30)    NOT NULL    DEFAULT 'FACE_NOT_ENROLLED',
        [embedding_id]          NVARCHAR(100)   NULL,
        [model_version]         NVARCHAR(50)    NULL,
        [pin_hash]              NVARCHAR(100)   NULL,
        [failed_pin_attempts]   INT             NOT NULL    DEFAULT 0,
        [pin_lockout_end]       DATETIME2       NULL,
        [enrolled_at]           DATETIME2       NULL,
        [pin_set_at]            DATETIME2       NULL,
        [is_active]             BIT             NOT NULL    DEFAULT 1,
        [created_at]            DATETIME2       NOT NULL    DEFAULT GETDATE(),
        [updated_at]            DATETIME2       NULL        DEFAULT GETDATE(),
        [created_by]            NVARCHAR(50)    NULL,
        [updated_by]            NVARCHAR(50)    NULL,
        
        CONSTRAINT [PK_user_face_profiles] PRIMARY KEY CLUSTERED ([id]),
        CONSTRAINT [FK_user_face_profiles_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[users]([id]) ON DELETE CASCADE,
        CONSTRAINT [UQ_user_face_profiles_user_id] UNIQUE ([user_id])
    );

    CREATE NONCLUSTERED INDEX [IX_user_face_profiles_user_id] ON [dbo].[user_face_profiles] ([user_id]);
    CREATE NONCLUSTERED INDEX [IX_user_face_profiles_status] ON [dbo].[user_face_profiles] ([status]) WHERE [is_active] = 1;

    PRINT 'Created table: user_face_profiles';
END
ELSE
BEGIN
    PRINT 'Table user_face_profiles already exists, skipping creation.';
END
GO
