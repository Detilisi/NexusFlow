CREATE PROCEDURE GetTransactions
    @AccountCode INT = -1,  
    @Code INT = -1    
AS
BEGIN
    SET NOCOUNT ON;

    SELECT t.*
    FROM Transactions t
    WHERE 
        (@AccountCode = -1 OR t.account_code = @AccountCode)
        AND (@Code = -1 OR t.code = @Code);
END;