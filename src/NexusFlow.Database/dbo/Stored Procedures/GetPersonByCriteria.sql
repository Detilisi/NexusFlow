CREATE PROCEDURE GetPersonByCriteria
    @SearchCriteria NVARCHAR(20), -- 'IdNumber', 'Surname', or 'AccountNumber'
    @SearchTerm NVARCHAR(50) -- The value to search for
AS
BEGIN
    SET NOCOUNT ON;

    -- Validate the search criteria
    IF @SearchCriteria NOT IN ('IdNumber', 'Surname', 'AccountNumber')
    BEGIN
        RAISERROR('Invalid search criteria. Must be IdNumber, Surname, or AccountNumber.', 16, 1);
        RETURN;
    END;

    -- Dynamic SQL to handle the criteria
    DECLARE @Sql NVARCHAR(MAX);

    SET @Sql = 
        CASE 
            WHEN @SearchCriteria = 'IdNumber' THEN
                'SELECT P.* 
                 FROM Persons P
                 WHERE P.id_number = @SearchTerm'
            WHEN @SearchCriteria = 'Surname' THEN
                'SELECT P.* 
                 FROM Persons P
                 WHERE P.surname = @SearchTerm'
            WHEN @SearchCriteria = 'AccountNumber' THEN
                'SELECT P.* 
                 FROM Persons P
                 INNER JOIN Accounts A ON P.code = A.person_code
                 WHERE A.account_number = @SearchTerm'
        END;

    -- Execute the constructed SQL
    EXEC sp_executesql @Sql, N'@SearchTerm NVARCHAR(50)', @SearchTerm;
END;
