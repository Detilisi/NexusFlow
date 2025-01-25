CREATE   PROCEDURE CreatePerson
    @Name NVARCHAR(50),
    @Surname NVARCHAR(50),
    @IdNumber NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if a person with the same ID number already exists
    IF EXISTS (SELECT 1 FROM Persons WHERE id_number = @IdNumber)
    BEGIN
        THROW 50001, 'A person with this ID number already exists.', 1;
    END

    -- Insert the new person
    INSERT INTO Persons (name, surname, id_number)
    VALUES (@Name, @Surname, @IdNumber);
END;