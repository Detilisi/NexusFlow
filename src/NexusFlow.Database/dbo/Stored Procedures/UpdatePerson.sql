CREATE   PROCEDURE UpdatePerson
    @PersonCode INT,
    @Name NVARCHAR(50) = NULL,
    @Surname NVARCHAR(50) = NULL,
    @IDNumber NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the person exists
    IF NOT EXISTS (SELECT 1 FROM Persons WHERE code = @PersonCode)
    BEGIN
        THROW 50002, 'Person not found.', 1;
    END

    -- Check if updating the ID number and it already exists for another person
    IF @IDNumber IS NOT NULL AND EXISTS (
        SELECT 1 FROM Persons WHERE id_number = @IDNumber AND code <> @PersonCode)
    BEGIN
        THROW 50003, 'The ID number is already assigned to another person.', 1;
    END

    -- Update the person
    UPDATE Persons
    SET name = COALESCE(@Name, name),
        surname = COALESCE(@Surname, surname),
        id_number = COALESCE(@IDNumber, id_number)
    WHERE code = @PersonCode;
END;