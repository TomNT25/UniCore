CREATE TRIGGER [dbo].[trg_whitelisted_emails_auto_code]
ON [dbo].[whitelisted_emails]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE w
    SET w.code = 'WLE-' + RIGHT('00000' + CAST((
        SELECT COUNT(*) FROM [dbo].[whitelisted_emails] w2 WHERE w2.created_at <= w.created_at
    ) AS VARCHAR(10)), 5)
    FROM [dbo].[whitelisted_emails] w
    INNER JOIN inserted i ON w.id = i.id
    WHERE w.code IS NULL OR w.code = '';
END;
