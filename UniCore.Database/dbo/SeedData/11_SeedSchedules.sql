PRINT 'Seeding Schedules...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-dbms-tue')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-dbms-tue', 'SCH-00001', 'cst-std1-dbms', '08:00-10:15', '2026-09-22 08:00:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-dsa-thu')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-dsa-thu', 'SCH-00002', 'cst-std1-dsa', '13:30-15:45', '2026-09-24 13:30:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-ds-mon')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-ds-mon', 'SCH-00003', 'cst-std3-ds', '08:00-10:15', '2026-09-21 08:00:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-ai-wed')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-ai-wed', 'SCH-00004', 'cst-std4-ai', '10:30-12:45', '2026-09-23 10:30:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-sec-fri')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-sec-fri', 'SCH-00005', 'cst-std5-sec', '13:30-15:45', '2026-09-25 13:30:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-ui-mon')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-ui-mon', 'SCH-00006', 'cst-std6-ui', '13:30-15:45', '2026-09-21 13:30:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-fin-tue')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-fin-tue', 'SCH-00007', 'cst-std7-fin', '10:30-12:45', '2026-09-22 10:30:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-web-tue')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-web-tue', 'SCH-00008', 'cst-std2-web', '13:30-15:45', '2026-09-22 13:30:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-web-sat')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-web-sat', 'SCH-00009', 'cst-std3-web', '08:00-10:15', '2026-09-26 08:00:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-algo-wed')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-algo-wed', 'SCH-00010', 'cst-std4-algo', '08:00-10:15', '2026-09-23 08:00:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-fin-thu')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-fin-thu', 'SCH-00011', 'cst-std5-fin', '15:00-17:15', '2026-09-24 15:00:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-algo-fri')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-algo-fri', 'SCH-00012', 'cst-std7-algo', '08:00-10:15', '2026-09-25 08:00:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-bio-mon')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-bio-mon', 'SCH-00013', 'cst-std8-bio', '08:00-10:15', '2026-09-21 08:00:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-me-tue')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-me-tue', 'SCH-00014', 'cst-std9-me', '10:30-12:45', '2026-09-22 10:30:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-ce-wed')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-ce-wed', 'SCH-00015', 'cst-std10-ce', '13:30-15:45', '2026-09-23 13:30:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-chem-thu')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-chem-thu', 'SCH-00016', 'cst-std11-chem', '08:00-10:15', '2026-09-24 08:00:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-arch-fri')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-arch-fri', 'SCH-00017', 'cst-std12-arch', '10:30-12:45', '2026-09-25 10:30:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-env-sat')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-env-sat', 'SCH-00018', 'cst-std13-env', '08:00-10:15', '2026-09-26 08:00:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-psy-mon')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-psy-mon', 'SCH-00019', 'cst-std14-psy', '13:30-15:45', '2026-09-21 13:30:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-iot-tue')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-iot-tue', 'SCH-00020', 'cst-std8-iot', '15:00-17:15', '2026-09-22 15:00:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-eng-wed')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-eng-wed', 'SCH-00021', 'cst-std9-eng', '08:00-10:15', '2026-09-23 08:00:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [id] = 'sch-logistics-thu')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-logistics-thu', 'SCH-00022', 'cst-std14-logistics', '13:30-15:45', '2026-09-24 13:30:00', 1, 0);
END;

GO
