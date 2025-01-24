CREATE PROCEDURE UpdateAccountNumber
    @account_code INT,               -- The code of the account to update
    @new_account_number VARCHAR(50)  -- The new account number
AS
BEGIN
    -- Check if the account exists
    IF NOT EXISTS (SELECT 1 FROM Accounts WHERE code = @account_code)
    BEGIN
        RAISERROR('Account not found.', 16, 1);
        RETURN;
    END

    -- Check if the account is closed
    IF (SELECT status_code FROM Accounts WHERE code = @account_code) = 2 -- 'Closed'
    BEGIN
        RAISERROR('Cannot update a closed account.', 16, 1);
        RETURN;
    END

    -- Check if the new account number already exists
    IF EXISTS (SELECT 1 FROM Accounts WHERE account_number = @new_account_number AND code <> @account_code)
    BEGIN
        RAISERROR('Account number already exists.', 16, 1);
        RETURN;
    END

    -- Update the account number
    UPDATE Accounts
    SET account_number = @new_account_number
    WHERE code = @account_code;

    -- Return the updated account details
    SELECT * FROM Accounts WHERE code = @account_code;
END