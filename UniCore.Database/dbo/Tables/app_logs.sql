CREATE TABLE [dbo].[app_logs] (
    [id]           VARCHAR (50)   CONSTRAINT [DF_app_logs_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [log_date]     DATETIME2 (7)  NOT NULL,
    [thread]       VARCHAR (255)  NULL,
    [log_level]    VARCHAR (50)   NOT NULL,
    [logger]       VARCHAR (255)  NULL,
    [message]      NVARCHAR (MAX) NULL,
    [exception]    NVARCHAR (MAX) NULL,
    [machine_name] VARCHAR (255)  NULL,
    [trace_id]     VARCHAR (255)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_app_logs_log_date]
    ON [dbo].[app_logs] ([log_date] DESC);
GO

CREATE NONCLUSTERED INDEX [IX_app_logs_level_date]
    ON [dbo].[app_logs] ([log_level] ASC, [log_date] DESC);
GO

CREATE NONCLUSTERED INDEX [IX_app_logs_trace_id]
    ON [dbo].[app_logs] ([trace_id] ASC)
    WHERE [trace_id] IS NOT NULL;
GO
