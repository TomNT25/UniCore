-- ==========================================
-- 1. Create the View (Required for NEWID)
-- ==========================================
CREATE VIEW dbo.vw_GenerateGuid
AS
SELECT NEWID() AS GuidValue;