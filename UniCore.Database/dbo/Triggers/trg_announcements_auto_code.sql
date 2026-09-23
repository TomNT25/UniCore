CREATE TRIGGER [dbo].[trg_announcements_auto_code]
ON [dbo].[announcements]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE a
    SET a.code = 'ANC-' + RIGHT('00000' + CAST((
        SELECT COUNT(*) FROM [dbo].[announcements] a2 WHERE a2.created_at <= a.created_at
    ) AS VARCHAR(10)), 5)
    FROM [dbo].[announcements] a
    INNER JOIN inserted i ON a.id = i.id
    WHERE a.code IS NULL OR a.code = '';
END;
