CREATE TRIGGER [dbo].[trg_permissions_auto_code]
ON [dbo].[permissions]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE p
    SET p.code = 'PRM-' + RIGHT('00000' + CAST((
        SELECT COUNT(*) FROM [dbo].[permissions] p2 WHERE p2.created_at <= p.created_at
    ) AS VARCHAR(10)), 5)
    FROM [dbo].[permissions] p
    INNER JOIN inserted i ON p.id = i.id
    WHERE p.code IS NULL OR p.code = '';
END;
