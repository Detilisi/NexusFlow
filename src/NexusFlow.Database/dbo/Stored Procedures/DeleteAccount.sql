CREATE PROCEDURE DeleteAccount
    @Code INT
AS
BEGIN
    -- Check if the account exists
    IF NOT EXISTS (SELECT 1 FROM Accounts WHERE code = @Code)
    BEGIN
        RAISERROR('Account not found.', 16, 1);
        RETURN;
    END

    -- Check if the account has a zero balance
    IF (SELECT outstanding_balance FROM Accounts WHERE code = @Code) <> 0
    BEGIN
        RAISERROR('Cannot delete account with a non-zero balance.', 16, 1);
        RETURN;
    END

    -- Check if the account has any transactions
    IF EXISTS (SELECT 1 FROM Transactions WHERE account_code = (SELECT code FROM Accounts WHERE code = @Code))
    BEGIN
        RAISERROR('Cannot delete account with transactions.', 16, 1);
        RETURN;
    END

    -- Delete the account
    DELETE FROM Accounts WHERE code = @Code;

    -- Return success message
    SELECT 'Account deleted successfully.' AS Message;
END