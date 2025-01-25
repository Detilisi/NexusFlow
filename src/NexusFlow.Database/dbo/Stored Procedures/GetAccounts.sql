CREATE PROCEDURE GetAccounts
    @PersonCode INT = -1,
    @Code INT = -1
AS
BEGIN
    SET NOCOUNT ON;

    SELECT a.*
    FROM Accounts a
    WHERE 
        (@PersonCode = -1 OR a.person_code = @PersonCode)
        AND (@Code = -1 OR a.code = @Code);
END;
GO