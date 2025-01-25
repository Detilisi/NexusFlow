CREATE PROCEDURE UpdateTransaction
    @Code INT,
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

    -- Rule 5: The transaction amount can never be zero (0).
    IF @Amount = 0
    BEGIN
        RAISERROR('Transaction amount cannot be zero.', 16, 1);
        RETURN;
    END;

    -- Update the transaction
    UPDATE [dbo].[Transactions]
    SET
        [transaction_date] = @TransactionDate,
        [amount] = @Amount,
        [description] = @Description,
		[capture_date] = GETDATE()
    WHERE
        [code] = @Code;

    -- Return the number of rows affected
    SELECT @@ROWCOUNT AS RowsAffected;
END;