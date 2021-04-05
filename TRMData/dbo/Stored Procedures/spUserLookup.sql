CREATE PROCEDURE [dbo].[spUserLookup]
	@Id nvarchar(128)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT AuthUserId, FirstName, LastName, EmailAddress, CreatedDate
	FROM [dbo].[Users]
	WHERE AuthUserId = @Id;
END
