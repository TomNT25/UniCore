CREATE TRIGGER [dbo].[trg_classes_auto_code]
ON [dbo].[classes]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE cls
    SET cls.code = 'CLS-' + RIGHT('00000' + CAST((
        SELECT COUNT(*) FROM [dbo].[classes] c2 WHERE c2.created_at <= cls.created_at
    ) AS VARCHAR(10)), 5)
    FROM [dbo].[classes] cls
    INNER JOIN inserted i ON cls.id = i.id
    WHERE cls.code IS NULL OR cls.code = '';
END;
