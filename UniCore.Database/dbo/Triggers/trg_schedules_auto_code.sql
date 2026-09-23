CREATE TRIGGER [dbo].[trg_schedules_auto_code]
ON [dbo].[schedules]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE s
    SET s.code = 'SCH-' + RIGHT('00000' + CAST((
        SELECT COUNT(*) FROM [dbo].[schedules] s2 WHERE s2.created_at <= s.created_at
    ) AS VARCHAR(10)), 5)
    FROM [dbo].[schedules] s
    INNER JOIN inserted i ON s.id = i.id
    WHERE s.code IS NULL OR s.code = '';
END;
