PRINT 'Seeding App Logs...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-001')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-001', sysutcdatetime(), '1', 'INFO', 'UniCore.API.Program', 'Application UniCore.API started successfully.', 'SERVER-PROD-01', 'trc-start-001');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-002')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-002', sysutcdatetime(), '1', 'INFO', 'UniCore.Database.Migration', 'DB Migration executed successfully.', 'SERVER-PROD-01', 'trc-mig-002');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-003')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-003', sysutcdatetime(), '2', 'INFO', 'UniCore.Infrastructure.Redis', 'Redis Cache connected successfully.', 'SERVER-PROD-01', 'trc-red-003');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-004')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-004', sysutcdatetime(), '3', 'INFO', 'UniCore.API.Services.GoogleAuthProvider', 'External Auth Google Provider Service initialized.', 'SERVER-PROD-01', 'trc-auth-004');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-005')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-005', sysutcdatetime(), '4', 'INFO', 'UniCore.API.BackgroundJobs', 'Background Jobs Worker started.', 'SERVER-PROD-01', 'trc-bg-005');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-006')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-006', sysutcdatetime(), '1', 'INFO', 'UniCore.API.Extensions.CorsExtension', 'CORS Policy loaded for localhost origins.', 'SERVER-PROD-01', 'trc-cors-006');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-007')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-007', sysutcdatetime(), '1', 'INFO', 'UniCore.Infrastructure.Security.Jwt', 'JWT Token Issuer configured with RS256 algorithm.', 'SERVER-PROD-01', 'trc-jwt-007');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-008')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-008', sysutcdatetime(), '2', 'INFO', 'UniCore.Infrastructure.Security.Mfa', 'MFA TOTP provider service registered.', 'SERVER-PROD-01', 'trc-mfa-008');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-009')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-009', sysutcdatetime(), '5', 'INFO', 'UniCore.API.Services.AnnouncementService', 'Announcement Notification Service batch job scheduled.', 'SERVER-PROD-01', 'trc-anc-009');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-010')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-010', sysutcdatetime(), '1', 'INFO', 'UniCore.API.HealthChecks', 'Student Enrollment Health Check passed.', 'SERVER-PROD-01', 'trc-chk-010');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-011')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-011', sysutcdatetime(), '3', 'INFO', 'UniCore.API.Hubs.NotificationHub', 'SignalR Realtime Hub connected.', 'SERVER-PROD-01', 'trc-hub-011');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-012')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-012', sysutcdatetime(), '1', 'INFO', 'UniCore.Database.Seeder', 'Database Seeder batch 2 loaded successfully.', 'SERVER-PROD-01', 'trc-seed-012');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-013')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-013', sysutcdatetime(), '2', 'INFO', 'UniCore.API.Services.EmailService', 'SMTP Relay connection established.', 'SERVER-PROD-01', 'trc-mail-013');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-014')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-014', sysutcdatetime(), '4', 'INFO', 'UniCore.API.Services.ReportService', 'Automated Daily Grade Report generated.', 'SERVER-PROD-01', 'trc-rpt-014');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-015')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-015', sysutcdatetime(), '1', 'WARN', 'UniCore.API.Middlewares.RateLimiting', 'Rate limiting threshold reached for IP 192.168.1.105.', 'SERVER-PROD-01', 'trc-rate-015');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-016')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-016', sysutcdatetime(), '3', 'INFO', 'UniCore.Infrastructure.Storage.S3', 'Cloud Storage bucket sync completed.', 'SERVER-PROD-01', 'trc-s3-016');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-017')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-017', sysutcdatetime(), '2', 'INFO', 'UniCore.API.Services.ScheduleService', 'Timetable slot collision check passed.', 'SERVER-PROD-01', 'trc-sch-017');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-018')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-018', sysutcdatetime(), '1', 'INFO', 'UniCore.API.Services.DormitoryService', 'Student room allocation sync executed.', 'SERVER-PROD-01', 'trc-drm-018');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-019')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-019', sysutcdatetime(), '5', 'INFO', 'UniCore.API.Services.TuitionService', 'Tuition billing cycle initialized for Fall 2026.', 'SERVER-PROD-01', 'trc-fin-019');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-020')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-020', sysutcdatetime(), '1', 'INFO', 'UniCore.API.Services.ExamService', 'Examination proctoring roster published.', 'SERVER-PROD-01', 'trc-exm-020');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[app_logs] WHERE [id] = 'alg-sys-021')
BEGIN
    INSERT INTO [dbo].[app_logs] ([id], [log_date], [thread], [log_level], [logger], [message], [machine_name], [trace_id])
    VALUES ('alg-sys-021', sysutcdatetime(), '4', 'INFO', 'UniCore.API.Program', 'All 14 seed script executions verified cleanly.', 'SERVER-PROD-01', 'trc-ver-021');
END;

GO
