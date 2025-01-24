CREATE   PROCEDURE CreatePerson
    @name NVARCHAR(50),
    @surname NVARCHAR(50),
    @id_number NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if a person with the same ID number already exists
    IF EXISTS (SELECT 1 FROM Persons WHERE id_number = @id_number)
    BEGIN
        THROW 50001, 'A person with this ID number already exists.', 1;
    END

    -- Insert the new person
    INSERT INTO Persons (name, surname, id_number)
    VALUES (@name, @surname, @id_number);
END;