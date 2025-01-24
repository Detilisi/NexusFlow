CREATE PROCEDURE GetPersonByCriteria
    @IDNumber NVARCHAR(20) = NULL,
    @Surname NVARCHAR(50) = NULL,
    @AccountNumber NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Retrieve the person based on the criteria
    SELECT DISTINCT p.*
    FROM Persons p
    LEFT JOIN Accounts a ON p.Code = a.person_code
    WHERE 
        (@IDNumber IS NULL OR p.Id_Number = @IDNumber) AND
        (@Surname IS NULL OR p.Surname = @Surname) AND
        (@AccountNumber IS NULL OR a.Account_Number = @AccountNumber);
END;
