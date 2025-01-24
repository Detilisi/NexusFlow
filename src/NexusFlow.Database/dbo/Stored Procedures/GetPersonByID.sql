CREATE   PROCEDURE GetPersonByID
    @IDNumber NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    -- Retrieve the person
    SELECT *
    FROM Persons
    WHERE id_number = @IDNumber;
END;