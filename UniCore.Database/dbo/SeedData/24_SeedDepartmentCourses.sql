PRINT 'Seeding 5 Departments and 30 Courses...';

-- ============================================================================
-- 1. Upsert Departments
-- ============================================================================

-- Mathematics - Informatics - Natural Sciences
IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [name] = N'Mathematics - Informatics - Natural Sciences')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-mins-025', 'DPT-00025', N'Mathematics - Informatics - Natural Sciences', N'Provides foundational knowledge in mathematics, computer science, and natural sciences for students across different academic disciplines.', 1, 0);
END
ELSE
BEGIN
    UPDATE [dbo].[departments]
    SET [description] = N'Provides foundational knowledge in mathematics, computer science, and natural sciences for students across different academic disciplines.'
    WHERE [name] = N'Mathematics - Informatics - Natural Sciences';
END;

-- Information Technology
IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [name] = N'Information Technology')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-it-002', 'DPT-00002', N'Information Technology', N'Focuses on software development, computer systems, databases, networks, and modern information technology.', 1, 0);
END
ELSE
BEGIN
    UPDATE [dbo].[departments]
    SET [description] = N'Focuses on software development, computer systems, databases, networks, and modern information technology.'
    WHERE [name] = N'Information Technology';
END;

-- Business Administration
IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [name] = N'Business Administration')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-ba-003', 'DPT-00003', N'Business Administration', N'Develops knowledge and skills in management, marketing, finance, operations, and organizational decision-making.', 1, 0);
END
ELSE
BEGIN
    UPDATE [dbo].[departments]
    SET [description] = N'Develops knowledge and skills in management, marketing, finance, operations, and organizational decision-making.'
    WHERE [name] = N'Business Administration';
END;

-- Electrical and Electronic Engineering
IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [name] = N'Electrical and Electronic Engineering')
BEGIN
    IF EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [name] = N'Electrical Engineering')
    BEGIN
        UPDATE [dbo].[departments]
        SET [name] = N'Electrical and Electronic Engineering',
            [description] = N'Covers electrical systems, electronics, control systems, signal processing, and embedded technologies.'
        WHERE [name] = N'Electrical Engineering';
    END
    ELSE
    BEGIN
        INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
        VALUES ('dpt-eee-026', 'DPT-00026', N'Electrical and Electronic Engineering', N'Covers electrical systems, electronics, control systems, signal processing, and embedded technologies.', 1, 0);
    END
END
ELSE
BEGIN
    UPDATE [dbo].[departments]
    SET [description] = N'Covers electrical systems, electronics, control systems, signal processing, and embedded technologies.'
    WHERE [name] = N'Electrical and Electronic Engineering';
END;

-- Mechanical Engineering
IF NOT EXISTS (SELECT 1 FROM [dbo].[departments] WHERE [name] = N'Mechanical Engineering')
BEGIN
    INSERT INTO [dbo].[departments] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('dpt-me-016', 'DPT-00016', N'Mechanical Engineering', N'Focuses on mechanical systems, materials, manufacturing, thermodynamics, fluid mechanics, and machine design.', 1, 0);
END
ELSE
BEGIN
    UPDATE [dbo].[departments]
    SET [description] = N'Focuses on mechanical systems, materials, manufacturing, thermodynamics, fluid mechanics, and machine design.'
    WHERE [name] = N'Mechanical Engineering';
END;

-- ============================================================================
-- 2. Resolve Department Foreign Keys
-- ============================================================================
DECLARE @DeptId_MINS VARCHAR(50);
DECLARE @DeptId_IT   VARCHAR(50);
DECLARE @DeptId_BA   VARCHAR(50);
DECLARE @DeptId_EEE  VARCHAR(50);
DECLARE @DeptId_ME   VARCHAR(50);

SELECT TOP 1 @DeptId_MINS = [id] FROM [dbo].[departments] WHERE [name] = N'Mathematics - Informatics - Natural Sciences' AND [is_deleted] = 0;
SELECT TOP 1 @DeptId_IT   = [id] FROM [dbo].[departments] WHERE [name] = N'Information Technology' AND [is_deleted] = 0;
SELECT TOP 1 @DeptId_BA   = [id] FROM [dbo].[departments] WHERE [name] = N'Business Administration' AND [is_deleted] = 0;
SELECT TOP 1 @DeptId_EEE  = [id] FROM [dbo].[departments] WHERE [name] = N'Electrical and Electronic Engineering' AND [is_deleted] = 0;
SELECT TOP 1 @DeptId_ME   = [id] FROM [dbo].[departments] WHERE [name] = N'Mechanical Engineering' AND [is_deleted] = 0;

-- ============================================================================
-- 3. Upsert Courses
-- ============================================================================
DECLARE @CoursesToSeed TABLE (
    [id]            VARCHAR(50),
    [code]          VARCHAR(50),
    [type]          VARCHAR(20),
    [name]          NVARCHAR(100),
    [description]   NVARCHAR(255),
    [department_id] VARCHAR(50)
);

INSERT INTO @CoursesToSeed ([id], [code], [type], [name], [description], [department_id])
VALUES
-- Mathematics - Informatics - Natural Sciences (COMMON)
('crs-la-101', 'CRS-00101', 'COMMON', N'Linear Algebra', N'Fundamental concepts of vectors, matrices, linear transformations, eigenvalues, and eigenvectors.', @DeptId_MINS),
('crs-dm-102', 'CRS-00102', 'COMMON', N'Discrete Mathematics', N'Fundamental concepts of logic, sets, relations, functions, graphs, combinatorics, and discrete structures.', @DeptId_MINS),
('crs-pas-103', 'CRS-00103', 'COMMON', N'Probability and Statistics', N'Introduction to probability theory, statistical methods, random variables, probability distributions, and data analysis.', @DeptId_MINS),
('crs-calc-104', 'CRS-00104', 'COMMON', N'Calculus', N'Fundamental concepts of limits, derivatives, integrals, sequences, series, and their applications.', @DeptId_MINS),
('crs-gp-105', 'CRS-00105', 'COMMON', N'General Physics', N'Introduction to fundamental principles of mechanics, thermodynamics, electricity, magnetism, and optics.', @DeptId_MINS),
('crs-itp-106', 'CRS-00106', 'COMMON', N'Introduction to Programming', N'Introduction to programming concepts, algorithms, data types, control structures, functions, and basic problem solving.', @DeptId_MINS),

-- Information Technology (SPECIFIC_DEPARTMENT)
('crs-it-dsa-107', 'CRS-00107', 'SPECIFIC_DEPARTMENT', N'Data Structures and Algorithms', N'Study of fundamental data structures and algorithms, including arrays, linked lists, trees, graphs, sorting, and searching.', @DeptId_IT),
('crs-it-dbs-108', 'CRS-00108', 'SPECIFIC_DEPARTMENT', N'Database Systems', N'Introduction to relational databases, SQL, database design, normalization, transactions, and database management systems.', @DeptId_IT),
('crs-it-os-109', 'CRS-00109', 'SPECIFIC_DEPARTMENT', N'Operating Systems', N'Study of operating system concepts including processes, threads, memory management, file systems, and resource management.', @DeptId_IT),
('crs-it-cn-110', 'CRS-00110', 'SPECIFIC_DEPARTMENT', N'Computer Networks', N'Fundamental concepts of computer networking, communication protocols, network architectures, routing, and network security.', @DeptId_IT),
('crs-it-se-111', 'CRS-00111', 'SPECIFIC_DEPARTMENT', N'Software Engineering', N'Principles and practices for designing, developing, testing, deploying, and maintaining reliable software systems.', @DeptId_IT),
('crs-it-wad-112', 'CRS-00112', 'SPECIFIC_DEPARTMENT', N'Web Application Development', N'Covers client-side and server-side web development, REST APIs, web frameworks, authentication, and application deployment.', @DeptId_IT),

-- Business Administration (SPECIFIC_DEPARTMENT)
('crs-ba-pom-113', 'CRS-00113', 'SPECIFIC_DEPARTMENT', N'Principles of Management', N'Introduction to management principles, organizational structures, leadership, planning, decision-making, and control.', @DeptId_BA),
('crs-ba-mm-114', 'CRS-00114', 'SPECIFIC_DEPARTMENT', N'Marketing Management', N'Study of marketing principles, consumer behavior, market research, segmentation, branding, and marketing strategies.', @DeptId_BA),
('crs-ba-fm-115', 'CRS-00115', 'SPECIFIC_DEPARTMENT', N'Financial Management', N'Introduction to financial decision-making, financial planning, investment analysis, capital budgeting, and risk management.', @DeptId_BA),
('crs-ba-om-116', 'CRS-00116', 'SPECIFIC_DEPARTMENT', N'Operations Management', N'Study of production processes, supply chains, quality management, inventory control, and operational efficiency.', @DeptId_BA),
('crs-ba-ob-117', 'CRS-00117', 'SPECIFIC_DEPARTMENT', N'Organizational Behavior', N'Explores individual and group behavior in organizations, motivation, communication, leadership, and workplace culture.', @DeptId_BA),
('crs-ba-bs-118', 'CRS-00118', 'SPECIFIC_DEPARTMENT', N'Business Strategy', N'Examines strategic planning, competitive analysis, business models, organizational capabilities, and long-term strategic decision-making.', @DeptId_BA),

-- Electrical and Electronic Engineering (SPECIFIC_DEPARTMENT)
('crs-ee-ca-119', 'CRS-00119', 'SPECIFIC_DEPARTMENT', N'Circuit Analysis', N'Fundamental analysis of electrical circuits using voltage, current, resistance, Kirchhoff''s laws, and circuit theorems.', @DeptId_EEE),
('crs-ee-de-120', 'CRS-00120', 'SPECIFIC_DEPARTMENT', N'Digital Electronics', N'Study of digital logic, Boolean algebra, logic gates, combinational circuits, sequential circuits, and digital systems.', @DeptId_EEE),
('crs-ee-ed-121', 'CRS-00121', 'SPECIFIC_DEPARTMENT', N'Electronic Devices', N'Introduction to semiconductor devices including diodes, transistors, operational amplifiers, and their applications.', @DeptId_EEE),
('crs-ee-ss-122', 'CRS-00122', 'SPECIFIC_DEPARTMENT', N'Signals and Systems', N'Study of continuous and discrete signals, linear systems, convolution, Fourier analysis, and system responses.', @DeptId_EEE),
('crs-ee-cs-123', 'CRS-00123', 'SPECIFIC_DEPARTMENT', N'Control Systems', N'Fundamental principles of feedback systems, system modeling, stability analysis, and controller design.', @DeptId_EEE),
('crs-ee-es-124', 'CRS-00124', 'SPECIFIC_DEPARTMENT', N'Embedded Systems', N'Introduction to embedded system architecture, microcontrollers, sensors, real-time programming, and hardware-software integration.', @DeptId_EEE),

-- Mechanical Engineering (SPECIFIC_DEPARTMENT)
('crs-me-em-125', 'CRS-00125', 'SPECIFIC_DEPARTMENT', N'Engineering Mechanics', N'Study of forces, moments, equilibrium, motion, and mechanical behavior of rigid bodies and engineering systems.', @DeptId_ME),
('crs-me-td-126', 'CRS-00126', 'SPECIFIC_DEPARTMENT', N'Thermodynamics', N'Fundamental principles of energy, heat, work, thermodynamic processes, and energy conversion systems.', @DeptId_ME),
('crs-me-fm-127', 'CRS-00127', 'SPECIFIC_DEPARTMENT', N'Fluid Mechanics', N'Study of fluid properties, fluid statics, fluid dynamics, flow behavior, and applications in engineering systems.', @DeptId_ME),
('crs-me-md-128', 'CRS-00128', 'SPECIFIC_DEPARTMENT', N'Machine Design', N'Principles of mechanical component design, including shafts, gears, bearings, fasteners, and mechanical assemblies.', @DeptId_ME),
('crs-me-mp-129', 'CRS-00129', 'SPECIFIC_DEPARTMENT', N'Manufacturing Processes', N'Introduction to machining, casting, forming, welding, additive manufacturing, and modern production technologies.', @DeptId_ME),
('crs-me-mat-130', 'CRS-00130', 'SPECIFIC_DEPARTMENT', N'Engineering Materials', N'Study of the properties, structures, selection, and applications of metals, polymers, ceramics, and composite materials.', @DeptId_ME);

-- 1. Update existing courses matching by name and department_id
UPDATE c
SET c.[type]        = s.[type],
    c.[description] = s.[description],
    c.[is_active]   = 1,
    c.[is_deleted]  = 0,
    c.[updated_at]  = GETDATE()
FROM [dbo].[courses] c
INNER JOIN @CoursesToSeed s 
    ON c.[name] = s.[name] AND c.[department_id] = s.[department_id];

-- 2. Insert new courses that do not exist by name+department_id and do not exist by id or code
INSERT INTO [dbo].[courses] ([id], [code], [type], [name], [description], [department_id], [is_active], [is_deleted])
SELECT s.[id], s.[code], s.[type], s.[name], s.[description], s.[department_id], 1, 0
FROM @CoursesToSeed s
WHERE NOT EXISTS (
    SELECT 1 FROM [dbo].[courses] c 
    WHERE (c.[name] = s.[name] AND c.[department_id] = s.[department_id])
       OR c.[id] = s.[id]
       OR c.[code] = s.[code]
);

GO
