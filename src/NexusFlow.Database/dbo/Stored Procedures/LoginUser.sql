CREATE   PROCEDURE LoginUser
    @Email NVARCHAR(50),
    @Password NVARCHAR(256) -- Plain text password
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @HashedPassword NVARCHAR(256) = CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', @Password), 1);

    SELECT *
    FROM Users
    WHERE email = @Email AND passwordHash = @HashedPassword;

END