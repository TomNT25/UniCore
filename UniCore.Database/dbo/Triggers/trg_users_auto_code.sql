CREATE TRIGGER [dbo].[trg_users_auto_code]
ON [dbo].[users]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE u
    SET u.code = 'USR-' + RIGHT('00000' + CAST((
        SELECT COUNT(*) FROM [dbo].[users] u2 WHERE u2.created_at <= u.created_at
    ) AS VARCHAR(10)), 5)
    FROM [dbo].[users] u
    INNER JOIN inserted i ON u.id = i.id
    WHERE u.code IS NULL OR u.code = '';
END;
