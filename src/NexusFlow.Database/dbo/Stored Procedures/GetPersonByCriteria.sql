CREATE PROCEDURE GetPersonByCriteria
    @id_number NVARCHAR(20) = NULL,
    @surname NVARCHAR(50) = NULL,
    @account_number NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Retrieve the person based on the criteria
    SELECT DISTINCT p.*
    FROM Persons p
    LEFT JOIN Accounts a ON p.Code = a.person_code
    WHERE 
        (@id_number IS NULL OR p.Id_Number = @id_number) AND
        (@surname IS NULL OR p.Surname = @surname) AND
        (@account_number IS NULL OR a.Account_Number = @account_number);
END;
