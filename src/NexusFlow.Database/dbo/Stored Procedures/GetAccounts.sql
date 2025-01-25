CREATE PROCEDURE GetAccounts
    @PersonCode INT = -1,
    @Code INT = -1
AS
BEGIN
    SET NOCOUNT ON;

    -- Scenario 1: Both @PersonCode and @Code = -1, return all records
    IF @PersonCode = -1 AND @Code = -1
    BEGIN
        SELECT a.*
        FROM Accounts a;
        RETURN;
    END;

    -- Scenario 2: @Code = -1 but @PersonCode != -1, return all accounts for the person
    IF @Code = -1 AND @PersonCode != -1
    BEGIN
        SELECT a.*
        FROM Accounts a
        WHERE a.person_code = @PersonCode;
        RETURN;
    END;

    -- Scenario 3: @PersonCode = -1 but @Code != -1, return the account with code = @Code
    IF @PersonCode = -1 AND @Code != -1
    BEGIN
        SELECT a.*
        FROM Accounts a
        WHERE a.code = @Code;
        RETURN;
    END;

    -- Scenario 4: Both @PersonCode and @Code != -1, return the matching record
    IF @PersonCode != -1 AND @Code != -1
    BEGIN
        SELECT a.*
        FROM Accounts a
        WHERE a.person_code = @PersonCode
          AND a.code = @Code;
        RETURN;
    END;
END;
GO
