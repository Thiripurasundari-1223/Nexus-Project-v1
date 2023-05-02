IF EXISTS (
		SELECT 1
		FROM Sys.procedures
		WHERE name = 'usp_CheckUserAuthorization'
		)
	DROP PROC usp_CheckUserAuthorization
GO

CREATE PROC usp_CheckUserAuthorization (
	@EmailAddress NVARCHAR(100)
	,@Mode NVARCHAR(200)
	,@APICall NVARCHAR(200)
	)
AS
DECLARE @UserID INT = (
		SELECT UserID
		FROM Users
		WHERE EmailAddress = @EmailAddress
		)
DECLARE @ModeOption NVARCHAR(20) = Upper(@mode)
DECLARE @Where NVARCHAR(200)

IF (
		@ModeOption = 'PUT'
		OR @ModeOption = 'POST'
		OR @ModeOption = 'GET'
		)
BEGIN
	SET @Where = 'in (1,2)'
END
ELSE IF (
		@ModeOption = 'PUT'
		OR @ModeOption = 'POST'
		)
BEGIN
	SET @Where = 'in (1,2)'
END
ELSE IF (@ModeOption = 'Delete')
BEGIN
	SET @Where = 'in (1)'
END

CREATE TABLE #RoleOptions (Opt INT);

IF @UserID > 0
BEGIN
	DECLARE @SQLString NVARCHAR(200);

	SET @SQLString = ' Insert into #RoleOptions Select ' + @APICall + ' from UserRoles UR Inner join RoleSetup RS on UR.RoleId=RS.RoleID Where UR.UserID=' + Cast(@UserID AS VARCHAR(10))

	EXEC (@SQLString)

	EXEC ('Select Count(1) from #RoleOptions where Opt ' + @Where)
END