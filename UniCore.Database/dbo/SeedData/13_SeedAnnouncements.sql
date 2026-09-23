PRINT 'Seeding Announcements & Announcement Logs/Whitelists...';

-- 1. Announcements
IF NOT EXISTS (SELECT 1 FROM [dbo].[announcements] WHERE [id] = 'anc-welcome-001')
BEGIN
    INSERT INTO [dbo].[announcements] ([id], [code], [title], [description], [content], [type], [status], [scope_type], [require_acknowledgement], [publish_date], [created_by])
    VALUES ('anc-welcome-001', 'ANC-00001', 'Welcome to Fall Semester 2026', 'Semester opening announcement', 'Welcome all students and faculty to Fall Semester 2026. Please check your course schedules.', 'NORMAL', 'PUBLISHED', 'ALL', 0, sysutcdatetime(), 'usr-admin-001');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcements] WHERE [id] = 'anc-exam-002')
BEGIN
    INSERT INTO [dbo].[announcements] ([id], [code], [title], [description], [content], [type], [status], [scope_type], [require_acknowledgement], [publish_date], [created_by])
    VALUES ('anc-exam-002', 'ANC-00002', 'Midterm Exam Schedule Notification', 'Important notice regarding midterm exams', 'Midterm examinations will take place from Oct 15 to Oct 22. Acknowledge this notice immediately.', 'IMPORTANT', 'PUBLISHED', 'COHORT', 1, sysutcdatetime(), 'usr-admin-001');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcements] WHERE [id] = 'anc-tuition-003')
BEGIN
    INSERT INTO [dbo].[announcements] ([id], [code], [title], [description], [content], [type], [status], [scope_type], [require_acknowledgement], [publish_date], [created_by])
    VALUES ('anc-tuition-003', 'ANC-00003', 'Tuition Fee Payment Deadline Fall 2026', 'Notice regarding tuition fee payment deadline', 'All students must settle Fall 2026 tuition fees prior to October 1st, 2026.', 'IMPORTANT', 'PUBLISHED', 'ALL', 1, sysutcdatetime(), 'usr-admin-001');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcements] WHERE [id] = 'anc-workshop-004')
BEGIN
    INSERT INTO [dbo].[announcements] ([id], [code], [title], [description], [content], [type], [status], [scope_type], [require_acknowledgement], [publish_date], [created_by])
    VALUES ('anc-workshop-004', 'ANC-00004', 'AI & Cloud Computing Tech Talk', 'Special workshop by industry experts', 'Join us on Friday for a technical seminar on AI Infrastructure and Cloud Solutions in Hall A.', 'NORMAL', 'PUBLISHED', 'ALL', 0, sysutcdatetime(), 'usr-lecturer-001');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcements] WHERE [id] = 'anc-reg-005')
BEGIN
    INSERT INTO [dbo].[announcements] ([id], [code], [title], [description], [content], [type], [status], [scope_type], [require_acknowledgement], [publish_date], [created_by])
    VALUES ('anc-reg-005', 'ANC-00005', 'Course Registration Opening for Spring 2027', 'Spring 2027 course portal schedule', 'Online course registration portal for Spring 2027 opens on Nov 15. Check prerequisite credits.', 'IMPORTANT', 'PUBLISHED', 'ALL', 0, sysutcdatetime(), 'usr-admin-001');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcements] WHERE [id] = 'anc-lib-006')
BEGIN
    INSERT INTO [dbo].[announcements] ([id], [code], [title], [description], [content], [type], [status], [scope_type], [require_acknowledgement], [publish_date], [created_by])
    VALUES ('anc-lib-006', 'ANC-00006', 'Library Extended Opening Hours', 'Midterm revision week library hours', 'Central Library will remain open 24/7 during midterm examination week for all students.', 'NORMAL', 'PUBLISHED', 'ALL', 0, sysutcdatetime(), 'usr-staff-001');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcements] WHERE [id] = 'anc-scholarship-007')
BEGIN
    INSERT INTO [dbo].[announcements] ([id], [code], [title], [description], [content], [type], [status], [scope_type], [require_acknowledgement], [publish_date], [created_by])
    VALUES ('anc-scholarship-007', 'ANC-00007', 'Academic Excellence Scholarship Application 2026', 'Call for merit scholarship applications', 'Students with GPA > 3.6 are invited to submit scholarship dossiers before Oct 30.', 'IMPORTANT', 'PUBLISHED', 'COHORT', 1, sysutcdatetime(), 'usr-head-001');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcements] WHERE [id] = 'anc-dorm-008')
BEGIN
    INSERT INTO [dbo].[announcements] ([id], [code], [title], [description], [content], [type], [status], [scope_type], [require_acknowledgement], [publish_date], [created_by])
    VALUES ('anc-dorm-008', 'ANC-00008', 'Dormitory Maintenance Notice', 'Scheduled electricity maintenance', 'Building B dormitory will undergo power grid maintenance on Sunday from 08:00 to 12:00.', 'NORMAL', 'PUBLISHED', 'ALL', 0, sysutcdatetime(), 'usr-dormmgr-001');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcements] WHERE [id] = 'anc-sports-009')
BEGIN
    INSERT INTO [dbo].[announcements] ([id], [code], [title], [description], [content], [type], [status], [scope_type], [require_acknowledgement], [publish_date], [created_by])
    VALUES ('anc-sports-009', 'ANC-00009', 'Annual Campus Sports Olympiad Registration', 'University sports festival', 'Register your class teams for soccer, basketball, and badminton tournaments starting Nov 1.', 'NORMAL', 'PUBLISHED', 'ALL', 0, sysutcdatetime(), 'usr-staff-001');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcements] WHERE [id] = 'anc-jobfair-010')
BEGIN
    INSERT INTO [dbo].[announcements] ([id], [code], [title], [description], [content], [type], [status], [scope_type], [require_acknowledgement], [publish_date], [created_by])
    VALUES ('anc-jobfair-010', 'ANC-00010', 'Autumn IT & Engineering Career Fair 2026', 'Corporate recruitment expo', 'Meet representatives from 50+ tech companies in Hall C on Nov 10. Bring your printed resumes.', 'IMPORTANT', 'PUBLISHED', 'ALL', 0, sysutcdatetime(), 'usr-advisor-001');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcements] WHERE [id] = 'anc-eval-011')
BEGIN
    INSERT INTO [dbo].[announcements] ([id], [code], [title], [description], [content], [type], [status], [scope_type], [require_acknowledgement], [publish_date], [created_by])
    VALUES ('anc-eval-011', 'ANC-00011', 'Course Teaching Evaluation Survey', 'Anonymous lecturer evaluation feedback', 'All students are requested to complete the online teaching evaluation before exam week.', 'IMPORTANT', 'PUBLISHED', 'ALL', 1, sysutcdatetime(), 'usr-admin-001');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcements] WHERE [id] = 'anc-health-012')
BEGIN
    INSERT INTO [dbo].[announcements] ([id], [code], [title], [description], [content], [type], [status], [scope_type], [require_acknowledgement], [publish_date], [created_by])
    VALUES ('anc-health-012', 'ANC-00012', 'Annual Student Health Examination', 'Mandatory medical checkup schedule', 'Checkup for Cohort 18 students takes place at Campus Clinic from Oct 5 to Oct 8.', 'NORMAL', 'PUBLISHED', 'COHORT', 0, sysutcdatetime(), 'usr-admin-001');
END;

-- 2. Announcement Students (Read/Acknowledgement Receipts)
IF NOT EXISTS (SELECT 1 FROM [dbo].[announcement_students] WHERE [id] = 'ast-std1-anc1')
BEGIN
    INSERT INTO [dbo].[announcement_students] ([id], [announcement_id], [student_id], [viewed_at], [acknowledged_at])
    VALUES ('ast-std1-anc1', 'anc-welcome-001', 'usr-student-001', sysutcdatetime(), NULL);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcement_students] WHERE [id] = 'ast-std1-anc2')
BEGIN
    INSERT INTO [dbo].[announcement_students] ([id], [announcement_id], [student_id], [viewed_at], [acknowledged_at])
    VALUES ('ast-std1-anc2', 'anc-exam-002', 'usr-student-001', sysutcdatetime(), sysutcdatetime());
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcement_students] WHERE [id] = 'ast-std3-anc1')
BEGIN
    INSERT INTO [dbo].[announcement_students] ([id], [announcement_id], [student_id], [viewed_at], [acknowledged_at])
    VALUES ('ast-std3-anc1', 'anc-welcome-001', 'usr-student-003', sysutcdatetime(), NULL);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcement_students] WHERE [id] = 'ast-std4-anc2')
BEGIN
    INSERT INTO [dbo].[announcement_students] ([id], [announcement_id], [student_id], [viewed_at], [acknowledged_at])
    VALUES ('ast-std4-anc2', 'anc-exam-002', 'usr-student-004', sysutcdatetime(), sysutcdatetime());
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcement_students] WHERE [id] = 'ast-std5-anc3')
BEGIN
    INSERT INTO [dbo].[announcement_students] ([id], [announcement_id], [student_id], [viewed_at], [acknowledged_at])
    VALUES ('ast-std5-anc3', 'anc-tuition-003', 'usr-student-005', sysutcdatetime(), sysutcdatetime());
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcement_students] WHERE [id] = 'ast-std8-anc11')
BEGIN
    INSERT INTO [dbo].[announcement_students] ([id], [announcement_id], [student_id], [viewed_at], [acknowledged_at])
    VALUES ('ast-std8-anc11', 'anc-eval-011', 'usr-student-008', sysutcdatetime(), sysutcdatetime());
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcement_students] WHERE [id] = 'ast-std9-anc10')
BEGIN
    INSERT INTO [dbo].[announcement_students] ([id], [announcement_id], [student_id], [viewed_at], [acknowledged_at])
    VALUES ('ast-std9-anc10', 'anc-jobfair-010', 'usr-student-009', sysutcdatetime(), NULL);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcement_students] WHERE [id] = 'ast-std10-anc8')
BEGIN
    INSERT INTO [dbo].[announcement_students] ([id], [announcement_id], [student_id], [viewed_at], [acknowledged_at])
    VALUES ('ast-std10-anc8', 'anc-dorm-008', 'usr-student-010', sysutcdatetime(), sysutcdatetime());
END;

-- 3. Announcement Email Logs
IF NOT EXISTS (SELECT 1 FROM [dbo].[announcement_email_logs] WHERE [id] = 'ael-std1-anc1')
BEGIN
    INSERT INTO [dbo].[announcement_email_logs] ([id], [announcement_id], [student_id], [email], [status], [sent_at])
    VALUES ('ael-std1-anc1', 'anc-welcome-001', 'usr-student-001', 'student1@unicore.edu.vn', 'SENT', sysutcdatetime());
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcement_email_logs] WHERE [id] = 'ael-std3-anc1')
BEGIN
    INSERT INTO [dbo].[announcement_email_logs] ([id], [announcement_id], [student_id], [email], [status], [sent_at])
    VALUES ('ael-std3-anc1', 'anc-welcome-001', 'usr-student-003', 'student3@unicore.edu.vn', 'SENT', sysutcdatetime());
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcement_email_logs] WHERE [id] = 'ael-std4-anc2')
BEGIN
    INSERT INTO [dbo].[announcement_email_logs] ([id], [announcement_id], [student_id], [email], [status], [sent_at])
    VALUES ('ael-std4-anc2', 'anc-exam-002', 'usr-student-004', 'student4@unicore.edu.vn', 'SENT', sysutcdatetime());
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcement_email_logs] WHERE [id] = 'ael-std8-anc11')
BEGIN
    INSERT INTO [dbo].[announcement_email_logs] ([id], [announcement_id], [student_id], [email], [status], [sent_at])
    VALUES ('ael-std8-anc11', 'anc-eval-011', 'usr-student-008', 'student8@unicore.edu.vn', 'SENT', sysutcdatetime());
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcement_email_logs] WHERE [id] = 'ael-std10-anc8')
BEGIN
    INSERT INTO [dbo].[announcement_email_logs] ([id], [announcement_id], [student_id], [email], [status], [sent_at])
    VALUES ('ael-std10-anc8', 'anc-dorm-008', 'usr-student-010', 'student10@unicore.edu.vn', 'SENT', sysutcdatetime());
END;

GO
