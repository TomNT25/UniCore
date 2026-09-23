PRINT 'Seeding Courses...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-dsa-001')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-dsa-001', 'CRS-00001', 'REQUIRED', 'Data Structures and Algorithms', 'Fundamental concepts of linear & non-linear data structures', 'dpt-se-001', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-dbms-002')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-dbms-002', 'CRS-00002', 'REQUIRED', 'Database Management Systems', 'Relational database design, 3NF normalization, SQL & EF Core', 'dpt-se-001', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-web-003')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-web-003', 'CRS-00003', 'ELECTIVE', 'Web Application Development', 'Building scalable web applications with ASP.NET Core & React', 'dpt-it-002', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-ai-004')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-ai-004', 'CRS-00004', 'REQUIRED', 'Artificial Intelligence Foundations', 'Introduction to machine learning, neural networks, and search algorithms', 'dpt-ai-005', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-ds-005')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-ds-005', 'CRS-00005', 'REQUIRED', 'Big Data & Data Mining', 'Data mining techniques, MapReduce, Hadoop, Spark and analytics', 'dpt-ds-004', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-sec-006')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-sec-006', 'CRS-00006', 'ELECTIVE', 'Network Security & Cryptography', 'Cryptographic protocols, SSL/TLS, firewalls, penetration testing', 'dpt-cy-006', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-ui-007')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-ui-007', 'CRS-00007', 'ELECTIVE', 'UI/UX Design & Prototyping', 'User experience research, wireframing, Figma prototyping, usability testing', 'dpt-gd-007', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-fin-008')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-fin-008', 'CRS-00008', 'REQUIRED', 'Financial Analytics & Modeling', 'Financial risk analysis, quantitative modeling, stock portfolio management', 'dpt-fin-008', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-mkt-009')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-mkt-009', 'CRS-00009', 'ELECTIVE', 'Digital Marketing Strategy', 'SEO, social media analytics, ad campaign optimization', 'dpt-mkt-009', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-algo-010')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-algo-010', 'CRS-00010', 'REQUIRED', 'Advanced Computational Algorithms', 'NP-completeness, dynamic programming, graph algorithms', 'dpt-cs-010', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-iot-011')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-iot-011', 'CRS-00011', 'ELECTIVE', 'Internet of Things Embedded Systems', 'Microcontroller programming, sensors, MQTT, wireless sensor networks', 'dpt-ee-011', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-eng-012')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-eng-012', 'CRS-00012', 'REQUIRED', 'Corporate Business English', 'Professional communication, negotiation, business report writing', 'dpt-eng-012', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-law-013')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-law-013', 'CRS-00013', 'ELECTIVE', 'Cyber Law & Data Privacy', 'GDPR, HIPAA, IP protection, digital copyright governance', 'dpt-law-013', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-med-014')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-med-014', 'CRS-00014', 'ELECTIVE', 'Healthcare Data Management', 'HL7, FHIR standard, electronic health record database architecture', 'dpt-med-014', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-bio-015')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-bio-015', 'CRS-00015', 'REQUIRED', 'Gene Engineering & CRISPR', 'Recombinant DNA technology and gene editing applications', 'dpt-bio-015', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-me-016')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-me-016', 'CRS-00016', 'REQUIRED', 'Industrial Robotics & Automation', 'Kinematics, PLC programming, robotic arm control systems', 'dpt-me-016', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-ce-017')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-ce-017', 'CRS-00017', 'REQUIRED', 'Structural Dynamics & Seismic Design', 'Earthquake engineering and reinforced concrete structures', 'dpt-ce-017', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-chem-018')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-chem-018', 'CRS-00018', 'ELECTIVE', 'Polymer & Material Science', 'Synthesis, characterization and industrial polymer processing', 'dpt-chem-018', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-arch-019')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-arch-019', 'CRS-00019', 'REQUIRED', 'Sustainable Urban Architecture', 'Green building design, BIM modeling, energy-efficient housing', 'dpt-arch-019', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-env-020')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-env-020', 'CRS-00020', 'REQUIRED', 'Climate Policy & Carbon Accounting', 'Environmental impact assessment and carbon trading mechanisms', 'dpt-env-020', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-psy-021')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-psy-021', 'CRS-00021', 'ELECTIVE', 'Cognitive Psychology & Neuroscience', 'Brain function, memory models, attention mechanisms, sensory perception', 'dpt-psy-021', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-media-022')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-media-022', 'CRS-00022', 'ELECTIVE', 'Broadcast Journalism & Video Editing', 'News reporting, studio broadcasting, video post-production', 'dpt-media-022', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-hotel-023')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-hotel-023', 'CRS-00023', 'ELECTIVE', 'International Hotel Management', 'Resort management, front-office operations, guest relations', 'dpt-hotel-023', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[courses] WHERE [id] = 'crs-logistics-024')
BEGIN
    INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
    VALUES ('crs-logistics-024', 'CRS-00024', 'REQUIRED', 'Global Freight Logistics', 'Maritime shipping, port management, customs regulation, supply chain optimization', 'dpt-logistics-024', 1, 0);
END;

GO
