CREATE  PROCEDURE LoginUser
    @Email NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Users
    WHERE email = @Email;

END