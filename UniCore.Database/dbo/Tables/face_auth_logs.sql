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
GO

-- Index for querying by user
CREATE NONCLUSTERED INDEX [IX_face_auth_logs_user_id] ON [dbo].[face_auth_logs] ([user_id]) WHERE [user_id] IS NOT NULL;
GO

-- Index for querying by action
CREATE NONCLUSTERED INDEX [IX_face_auth_logs_action] ON [dbo].[face_auth_logs] ([action]);
GO

-- Index for querying by time (for rate limiting and analytics)
CREATE NONCLUSTERED INDEX [IX_face_auth_logs_created_at] ON [dbo].[face_auth_logs] ([created_at] DESC);
GO

-- Index for rate limiting by IP
CREATE NONCLUSTERED INDEX [IX_face_auth_logs_ip_action_time] ON [dbo].[face_auth_logs] ([ip_address], [action], [created_at]) WHERE [ip_address] IS NOT NULL;
GO

EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Audit log for face authentication actions. Does NOT store images or embeddings.', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'face_auth_logs';
GO
