-- 3. Procedure to Delete a Person
CREATE   PROCEDURE DeletePerson
    @Code INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the person exists
    IF NOT EXISTS (SELECT 1 FROM Persons WHERE code = @Code)
    BEGIN
        THROW 50004, 'Person not found.', 1;
    END

    -- Check if the person has active accounts
    IF EXISTS (
        SELECT 1 FROM Accounts 
        WHERE person_code = @Code AND outstanding_balance <> 0
    )
    BEGIN
        THROW 50005, 'Cannot delete a person with active accounts.', 1;
    END

    -- Delete the person
    DELETE FROM Persons WHERE code = @Code;
END;