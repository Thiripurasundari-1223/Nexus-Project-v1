-- =============================================
-- Author:  <vindhya>
-- Create date: <2020 09 09>
-- Description: <Get Account Status Report>
-- =============================================
IF (OBJECT_ID('GetAccountReport') IS NOT NULL)
  DROP PROCEDURE [GetAccountReport]
GO

CREATE PROCEDURE [GetAccountReport]
@ResourseId Int=0
AS
BEGIN
    BEGIN TRY   
		select Cast(ROW_NUMBER() Over(Order By AD.AccountStatus) as Int) as Id, AD.AccountStatus as Status, COUNT(AD.AccountId) as [Count] 
		from PMSNexus_Accounts.[dbo].[AccountDetails] AD Group By
		AD.AccountStatus Order By AD.AccountStatus 

		select AD.AccountId,AD.AccountName,CONCAT(AD.DirectorFirstName,' ',AD.DirectorLastName) As OwnerName,
		(Select Count(PD.ProjectId) from PMSNexus_Projects.[dbo].[ProjectDetails] PD where PD.AccountId=AD.AccountId AND PD.ProjectStatus='Ongoing') as ProjectCount,
		CONCAT(AD.ContactPersonFirstName,' ',AD.ContactPersonLastName) as ContactPersonName, AD.AccountStatus
		from PMSNexus_Accounts.[dbo].[AccountDetails] AD
     END TRY
     BEGIN CATCH
        EXEC [InsertSqlExceptionLog]
     END CATCH
END
GO