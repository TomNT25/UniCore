CREATE TRIGGER [dbo].[trg_courses_auto_code]
ON [dbo].[courses]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE c
    SET c.code = 'CRS-' + RIGHT('00000' + CAST((
        SELECT COUNT(*) FROM [dbo].[courses] c2 WHERE c2.created_at <= c.created_at
    ) AS VARCHAR(10)), 5)
    FROM [dbo].[courses] c
    INNER JOIN inserted i ON c.id = i.id
    WHERE c.code IS NULL OR c.code = '';
END;
