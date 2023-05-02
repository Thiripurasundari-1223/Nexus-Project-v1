-- =============================================
-- Author:  <vindhya>
-- Create date: <2020 09 09>
-- Description: <Get Project Status Report>
-- =============================================
IF (OBJECT_ID('GetProjectReport') IS NOT NULL)
  DROP PROCEDURE [GetProjectReport]
GO

CREATE PROCEDURE [GetProjectReport]
@ResourseId Int=0
AS

BEGIN
    BEGIN TRY   
		select Cast(ROW_NUMBER() Over(Order By PD.ProjectStatus) as Int) as Id, PD.ProjectStatus as Status, 
		COUNT(PD.ProjectId) as [Count] from PMSNexus_Projects.[dbo].[ProjectDetails] PD Group By
		PD.ProjectStatus Order By PD.ProjectStatus 
		
        SELECT PD.[ProjectId] , PD.[ProjectName] , PT.[ProjectTypeDescription] AS ProjectType , AD.[AccountName] , 
		CONCAT(AD.DirectorFirstName,' ',AD.DirectorLastName)As OwnerName ,      		
		CONCAT(U.FirstName,' ',U.LastName)AS ProjectSPOC , PD.[ProjectStartdate] , PD.[ProjectEndDate] , 
	          PD.[ProjectDuration] , PD.[ProjectStatus]
        FROM PMSNexus_Projects.[dbo].[ProjectDetails] PD       
	    JOIN PMSNexus_Accounts.[dbo].[AccountDetails] AD ON AD.[AccountId]=PD.[AccountId]
		JOIN PMSNexus_Projects.[dbo].[ProjectType] PT ON PT.[ProjectTypeId]=PD.[ProjectTypeId]
		JOIN PMSNexus_Employees.[dbo].[Employees] U ON U.EmployeeId=PD.ProjectSPOC
     END TRY
     BEGIN CATCH
        EXEC [InsertSqlExceptionLog]
     END CATCH
END
GO