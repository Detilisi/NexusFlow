
-- 5. Procedure to Retrieve All Persons
CREATE   PROCEDURE GetPersons
    @Code INT = -1
AS
BEGIN
    SET NOCOUNT ON;

    IF @Code <> -1
    BEGIN
        -- Retrieve the person with the specified code
        SELECT *
        FROM Persons
        WHERE code = @Code;
        RETURN;
    END;
    ELSE
    BEGIN
        -- Retrieve all persons
        SELECT *
        FROM Persons;
    END;
END;