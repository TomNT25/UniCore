CREATE TRIGGER [dbo].[trg_roles_auto_code]
ON [dbo].[roles]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE r
    SET r.code = 'ROL-' + RIGHT('00000' + CAST((
        SELECT COUNT(*) FROM [dbo].[roles] r2 WHERE r2.created_at <= r.created_at
    ) AS VARCHAR(10)), 5)
    FROM [dbo].[roles] r
    INNER JOIN inserted i ON r.id = i.id
    WHERE r.code IS NULL OR r.code = '';
END;
