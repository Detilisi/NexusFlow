CREATE PROCEDURE UpdateAccountStatus
    @account_code INT, 
    @new_status_code INT
AS
BEGIN
    -- Check if the account exists
    IF NOT EXISTS (SELECT 1 FROM Accounts WHERE code = @account_code)
    BEGIN
        RAISERROR('Account not found.', 16, 1);
        RETURN;
    END

    -- Check if the new status is valid
    IF NOT EXISTS (SELECT 1 FROM Status WHERE code = @new_status_code)
    BEGIN
        RAISERROR('Invalid status code.', 16, 1);
        RETURN;
    END

    -- Prevent closing an account with a non-zero balance
    IF @new_status_code = 2 AND (SELECT outstanding_balance FROM Accounts WHERE code = @account_code) <> 0
    BEGIN
        RAISERROR('Cannot close account with a non-zero balance.', 16, 1);
        RETURN;
    END

    -- Update the account status
    UPDATE Accounts
    SET status_code = @new_status_code
    WHERE code = @account_code;

    -- Return success message
    SELECT 'Account status updated successfully.' AS Message;
END