CREATE TRIGGER [dbo].[trg_student_class_courses_auto_code]
ON [dbo].[student_class_courses]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE scc
    SET scc.code = 'SCC-' + RIGHT('00000' + CAST((
        SELECT COUNT(*) FROM [dbo].[student_class_courses] scc2 WHERE scc2.created_at <= scc.created_at
    ) AS VARCHAR(10)), 5)
    FROM [dbo].[student_class_courses] scc
    INNER JOIN inserted i ON scc.id = i.id
    WHERE scc.code IS NULL OR scc.code = '';
END;
