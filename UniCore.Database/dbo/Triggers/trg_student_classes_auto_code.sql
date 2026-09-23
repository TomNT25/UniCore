CREATE TRIGGER [dbo].[trg_student_classes_auto_code]
ON [dbo].[student_classes]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE sc
    SET sc.code = 'SLC-' + RIGHT('00000' + CAST((
        SELECT COUNT(*) FROM [dbo].[student_classes] sc2 WHERE sc2.created_at <= sc.created_at
    ) AS VARCHAR(10)), 5)
    FROM [dbo].[student_classes] sc
    INNER JOIN inserted i ON sc.id = i.id
    WHERE sc.code IS NULL OR sc.code = '';
END;
