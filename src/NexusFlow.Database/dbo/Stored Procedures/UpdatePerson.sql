CREATE   PROCEDURE UpdatePerson
    @code INT,
    @name NVARCHAR(50) = NULL,
    @surname NVARCHAR(50) = NULL,
    @id_number NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the person exists
    IF NOT EXISTS (SELECT 1 FROM Persons WHERE code = @code)
    BEGIN
        THROW 50002, 'Person not found.', 1;
    END

    -- Check if updating the ID number and it already exists for another person
    IF @id_number IS NOT NULL AND EXISTS (
        SELECT 1 FROM Persons WHERE id_number = @id_number AND code <> @code)
    BEGIN
        THROW 50003, 'The ID number is already assigned to another person.', 1;
    END

    -- Update the person
    UPDATE Persons
    SET name = COALESCE(@name, name),
        surname = COALESCE(@surname, surname),
        id_number = COALESCE(@id_number, id_number)
    WHERE code = @code;
END;