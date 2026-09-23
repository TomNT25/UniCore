CREATE TRIGGER [dbo].[trg_course_students_auto_code]
ON [dbo].[course_students]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE cs
    SET cs.code = 'CST-' + RIGHT('00000' + CAST((
        SELECT COUNT(*) FROM [dbo].[course_students] cs2 WHERE cs2.created_at <= cs.created_at
    ) AS VARCHAR(10)), 5)
    FROM [dbo].[course_students] cs
    INNER JOIN inserted i ON cs.id = i.id
    WHERE cs.code IS NULL OR cs.code = '';
END;
