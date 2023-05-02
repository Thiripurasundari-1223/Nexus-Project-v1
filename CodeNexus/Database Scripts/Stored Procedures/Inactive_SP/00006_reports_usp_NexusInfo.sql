IF (OBJECT_ID('usp_NexusInfo') IS NOT NULL)
  DROP PROCEDURE usp_NexusInfo
GO

CREATE PROCEDURE usp_NexusInfo
AS
	DECLARE @Account INT, @Project INT, @User INT
	SELECT @Account = COUNT(1) FROM PMSNexus_Accounts.[dbo].AccountDetails
	SELECT @Project = COUNT(1) FROM PMSNexus_Projects.[dbo].ProjectDetails
	SELECT @User = COUNT(1) FROM PMSNexus_Employees.[dbo].[Employees]
	SELECT @Account NoOfAccounts, @Project NoOfProjects, @User NoOfUsers
GO