PRINT 'Seeding Departments...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-se-001')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-se-001', 'DPT-00001', 'Software Engineering', 'Department of Software Engineering and Computing Sciences', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-it-002')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-it-002', 'DPT-00002', 'Information Technology', 'Department of Information Systems and Networking', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-ba-003')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-ba-003', 'DPT-00003', 'Business Administration', 'Department of International Business and Administration', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-ds-004')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-ds-004', 'DPT-00004', 'Data Science', 'Department of Data Analytics, Big Data and Statistics', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-ai-005')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-ai-005', 'DPT-00005', 'Artificial Intelligence', 'Department of Machine Learning, Deep Learning and Robotics', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-cy-006')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-cy-006', 'DPT-00006', 'Cyber Security', 'Department of Information Assurance and Network Security', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-gd-007')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-gd-007', 'DPT-00007', 'Graphic Design', 'Department of Digital Design and Multimedia Arts', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-fin-008')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-fin-008', 'DPT-00008', 'Finance & Banking', 'Department of International Finance, Accounting and Banking', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-mkt-009')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-mkt-009', 'DPT-00009', 'Digital Marketing', 'Department of E-Commerce and Digital Marketing', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-cs-010')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-cs-010', 'DPT-00010', 'Computer Science', 'Department of Algorithms, Computation and Theoretical CS', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-ee-011')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-ee-011', 'DPT-00011', 'Electrical Engineering', 'Department of IoT, Microelectronics and Systems Engineering', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-eng-012')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-eng-012', 'DPT-00012', 'Business English', 'Department of Applied Linguistics and Global Communication', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-law-013')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-law-013', 'DPT-00013', 'Cyber Law & Governance', 'Department of Technology Policy and Legal Studies', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-med-014')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-med-014', 'DPT-00014', 'Health Informatics', 'Department of Medical Data Systems and Healthcare Tech', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-bio-015')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-bio-015', 'DPT-00015', 'Biotechnology', 'Department of Gene Technology and Molecular Biology', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-me-016')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-me-016', 'DPT-00016', 'Mechanical Engineering', 'Department of Robotics, Automation and Mechatronics', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-ce-017')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-ce-017', 'DPT-00017', 'Civil Engineering', 'Department of Urban Infrastructure and Structural Design', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-chem-018')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-chem-018', 'DPT-00018', 'Chemical Engineering', 'Department of Industrial Materials and Process Chemistry', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-arch-019')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-arch-019', 'DPT-00019', 'Architecture', 'Department of Architectural Design and Spatial Planning', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-env-020')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-env-020', 'DPT-00020', 'Environmental Science', 'Department of Sustainability and Climate Engineering', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-psy-021')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-psy-021', 'DPT-00021', 'Applied Psychology', 'Department of Behavioral Science and Cognitive Studies', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-media-022')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-media-022', 'DPT-00022', 'Mass Media & Journalism', 'Department of Public Relations and Broadcast Media', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-hotel-023')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-hotel-023', 'DPT-00023', 'Tourism & Hospitality', 'Department of Hotel Operations and Event Planning', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [id] = 'dpt-logistics-024')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-logistics-024', 'DPT-00024', 'Supply Chain & Logistics', 'Department of Global Supply Chain and Freight Operations', 1, 0);
END;

GO
