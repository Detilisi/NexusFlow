
-- 5. Procedure to Retrieve All Persons
CREATE   PROCEDURE GetAllPersons
AS
BEGIN
    SET NOCOUNT ON;

    -- Retrieve all persons
    SELECT *
    FROM Persons;
END;