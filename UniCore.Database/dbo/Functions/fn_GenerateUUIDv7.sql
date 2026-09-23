
-- ==========================================
-- 2. Create the UUIDv7 Function
-- ==========================================
CREATE FUNCTION dbo.fn_GenerateUUIDv7()
RETURNS VARCHAR(36)
AS
BEGIN
    -- 1. Get current Unix timestamp in milliseconds
    DECLARE @unix_ms BIGINT = DATEDIFF_BIG(MILLISECOND, '1970-01-01 00:00:00.000', SYSUTCDATETIME());
    
    -- 2. Convert to Hex and grab the 12 rightmost characters (48 bits)
    DECLARE @time_hex VARCHAR(12) = RIGHT(CONVERT(VARCHAR(34), CONVERT(VARBINARY(8), @unix_ms), 1), 12);
    
    -- 3. Get a random GUID without hyphens for the remaining entropy
    DECLARE @rand_hex VARCHAR(32) = (
        SELECT REPLACE(CONVERT(VARCHAR(36), GuidValue), '-', '') 
        FROM dbo.vw_GenerateGuid
    );
    
    -- 4. Assemble the UUID v7 string 
    RETURN 
        SUBSTRING(@time_hex, 1, 8) + '-' +
        SUBSTRING(@time_hex, 9, 4) + '-7' +
        SUBSTRING(@rand_hex, 1, 3) + '-8' +
        SUBSTRING(@rand_hex, 4, 3) + '-' +
        SUBSTRING(@rand_hex, 7, 12);
END;