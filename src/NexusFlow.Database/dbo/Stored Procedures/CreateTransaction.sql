CREATE PROCEDURE CreateTransaction
    @AccountCode INT,
    @TransactionDate DATETIME,
    @Amount MONEY,
    @Description VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    -- Rule 1: The transaction date can never be in the future.
    IF @TransactionDate > GETDATE()
    BEGIN
        RAISERROR('Transaction date cannot be in the future.', 16, 1);
        RETURN;
    END;

    -- Rule 2: New transactions can only be added after the account is created.
    IF NOT EXISTS (SELECT 1 FROM Accounts WHERE code = @AccountCode)
    BEGIN
        RAISERROR('Account does not exist. Transactions can only be added for existing accounts.', 16, 1);
        RETURN;
    END;

    -- Rule 3: The user is never allowed to change the capture date.
    -- The capture date is automatically set to the current date and time.
    DECLARE @capture_date DATETIME = GETDATE();

    -- Rule 4: Users may enter debit or credit amounts for a transaction.
    -- Rule 5: The transaction amount can never be zero (0).
    IF @Amount = 0
    BEGIN
        RAISERROR('Transaction amount cannot be zero.', 16, 1);
        RETURN;
    END;

    -- Insert the transaction
    INSERT INTO [dbo].[Transactions] (
        [account_code],
        [transaction_date],
        [capture_date],
        [amount],
        [description]
    )
    VALUES (
        @AccountCode,
        @TransactionDate,
        @capture_date,
        @Amount,
        @description
    );

    -- Return the newly created transaction code
    SELECT SCOPE_IDENTITY() AS NewTransactionCode;
END;