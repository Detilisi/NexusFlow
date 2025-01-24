CREATE PROCEDURE GetAccountsForPerson
    @person_code INT,
    @account_code INT = NULL -- Optional parameter
AS
BEGIN
    -- Retrieve accounts based on the provided parameters
    IF @account_code IS NULL
    BEGIN
        -- Return all accounts for the person
        SELECT a.*
        FROM Accounts a
        WHERE a.person_code = @person_code;
    END
    ELSE
    BEGIN
        -- Return a specific account for the person
        SELECT a.*
        FROM Accounts a
        WHERE a.person_code = @person_code
          AND a.code = @account_code;
    END
END
GO