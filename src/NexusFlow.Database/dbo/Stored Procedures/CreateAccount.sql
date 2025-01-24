CREATE PROCEDURE CreateAccount
    @person_code INT,
    @account_number VARCHAR(50)
AS
BEGIN
    -- Check if the person exists
    IF NOT EXISTS (SELECT 1 FROM Persons WHERE code = @person_code)
    BEGIN
        RAISERROR('Person not found.', 16, 1);
        RETURN;
    END

    -- Check if the account number already exists
    IF EXISTS (SELECT 1 FROM Accounts WHERE account_number = @account_number)
    BEGIN
        RAISERROR('Account number already exists.', 16, 1);
        RETURN;
    END

    -- Insert the new account (default status is 'Open')
    INSERT INTO Accounts (person_code, account_number, outstanding_balance, status_code)
    VALUES (@person_code, @account_number, 0, 1); -- Default outstanding_balance is 0

    -- Return the newly created account
    SELECT * FROM Accounts WHERE account_number = @account_number;
END