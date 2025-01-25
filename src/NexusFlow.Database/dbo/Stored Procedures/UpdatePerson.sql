CREATE   PROCEDURE UpdatePerson
    @Code INT,
    @Name NVARCHAR(50) = NULL,
    @Surname NVARCHAR(50) = NULL,
    @IdNumber NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the person exists
    IF NOT EXISTS (SELECT 1 FROM Persons WHERE code = @Code)
    BEGIN
        THROW 50002, 'Person not found.', 1;
    END

    -- Check if updating the ID number and it already exists for another person
    IF @IdNumber IS NOT NULL AND EXISTS (
        SELECT 1 FROM Persons WHERE id_number = @IdNumber AND code <> @Code)
    BEGIN
        THROW 50003, 'The ID number is already assigned to another person.', 1;
    END

    -- Update the person
    UPDATE Persons
    SET name = COALESCE(@Name, name),
        surname = COALESCE(@Surname, surname),
        id_number = COALESCE(@IdNumber, id_number)
    WHERE code = @Code;
END;