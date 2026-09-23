-- Migration: Create face_auth_logs table for audit logging
-- Phase 4 of Face Recognition Auth feature

-- Check if table already exists
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'face_auth_logs' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[face_auth_logs]
    (
        [id]            NVARCHAR(50)    NOT NULL    DEFAULT dbo.fn_GenerateUUIDv7(),
        [request_id]    NVARCHAR(50)    NOT NULL,
        [user_id]       NVARCHAR(50)    NULL,
        [action]        NVARCHAR(30)    NOT NULL,
        [result]        NVARCHAR(20)    NOT NULL,
        [error_code]    NVARCHAR(50)    NULL,
        [model_version] NVARCHAR(50)    NULL,
        [similarity]    FLOAT           NULL,
        [latency_ms]    INT             NULL,
        [ip_address]    NVARCHAR(50)    NULL,
        [user_agent]    NVARCHAR(500)   NULL,
        [metadata]      NVARCHAR(MAX)   NULL,
        [created_at]    DATETIME2       NOT NULL    DEFAULT GETDATE(),
        
        CONSTRAINT [PK_face_auth_logs] PRIMARY KEY CLUSTERED ([id])
    );

    CREATE NONCLUSTERED INDEX [IX_face_auth_logs_user_id] ON [dbo].[face_auth_logs] ([user_id]) WHERE [user_id] IS NOT NULL;
    CREATE NONCLUSTERED INDEX [IX_face_auth_logs_action] ON [dbo].[face_auth_logs] ([action]);
    CREATE NONCLUSTERED INDEX [IX_face_auth_logs_created_at] ON [dbo].[face_auth_logs] ([created_at] DESC);
    CREATE NONCLUSTERED INDEX [IX_face_auth_logs_ip_action_time] ON [dbo].[face_auth_logs] ([ip_address], [action], [created_at]) WHERE [ip_address] IS NOT NULL;

    PRINT 'Created table: face_auth_logs';
END
ELSE
BEGIN
    PRINT 'Table face_auth_logs already exists, skipping creation.';
END
GO
