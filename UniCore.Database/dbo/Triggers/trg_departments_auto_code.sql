CREATE TRIGGER [dbo].[trg_departments_auto_code]
ON [dbo].[departments]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE d
    SET d.code = 'DPT-' + RIGHT('00000' + CAST((
        SELECT COUNT(*) FROM [dbo].[departments] d2 WHERE d2.created_at <= d.created_at
    ) AS VARCHAR(10)), 5)
    FROM [dbo].[departments] d
    INNER JOIN inserted i ON d.id = i.id
    WHERE d.code IS NULL OR d.code = '';
END;
