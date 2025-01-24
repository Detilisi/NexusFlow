CREATE PROCEDURE DeleteAccount
    @account_number VARCHAR(50)
AS
BEGIN
    -- Check if the account exists
    IF NOT EXISTS (SELECT 1 FROM Accounts WHERE account_number = @account_number)
    BEGIN
        RAISERROR('Account not found.', 16, 1);
        RETURN;
    END

    -- Check if the account has a zero balance
    IF (SELECT outstanding_balance FROM Accounts WHERE account_number = @account_number) <> 0
    BEGIN
        RAISERROR('Cannot delete account with a non-zero balance.', 16, 1);
        RETURN;
    END

    -- Check if the account has any transactions
    IF EXISTS (SELECT 1 FROM Transactions WHERE account_code = (SELECT code FROM Accounts WHERE account_number = @account_number))
    BEGIN
        RAISERROR('Cannot delete account with transactions.', 16, 1);
        RETURN;
    END

    -- Delete the account
    DELETE FROM Accounts WHERE account_number = @account_number;

    -- Return success message
    SELECT 'Account deleted successfully.' AS Message;
END