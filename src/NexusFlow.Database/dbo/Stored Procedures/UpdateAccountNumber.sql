CREATE PROCEDURE UpdateAccountNumber
    @Code INT,               
    @AccountNumber NVARCHAR(50)
AS
BEGIN
    -- Check if the account exists
    IF NOT EXISTS (SELECT 1 FROM Accounts WHERE code = @Code)
    BEGIN
        RAISERROR('Account not found.', 16, 1);
        RETURN;
    END

    -- Check if the account is closed
    IF (SELECT status_code FROM Accounts WHERE code = @Code) = 2 -- 'Closed'
    BEGIN
        RAISERROR('Cannot update a closed account.', 16, 1);
        RETURN;
    END

    -- Check if the new account number already exists
    IF EXISTS (SELECT 1 FROM Accounts WHERE account_number = @AccountNumber AND code <> @Code)
    BEGIN
        RAISERROR('Account number already exists.', 16, 1);
        RETURN;
    END

    -- Update the account number
    UPDATE Accounts
    SET account_number = @AccountNumber
    WHERE code = @Code;

    -- Return the updated account details
    SELECT * FROM Accounts WHERE code = @Code;
END