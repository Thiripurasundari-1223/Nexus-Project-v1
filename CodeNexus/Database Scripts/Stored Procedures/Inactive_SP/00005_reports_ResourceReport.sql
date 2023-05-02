-- =============================================
-- Author:  <vindhya>
-- Create date: <2020 09 18>
-- Description: <Get Resource Report>
-- =============================================
IF (OBJECT_ID('GetResourceReport') IS NOT NULL)
  DROP PROCEDURE [GetResourceReport]
GO

CREATE PROCEDURE [GetResourceReport]
@ResourseId Int=NULL
AS
BEGIN
    BEGIN TRY
-----------------------------------Resource Grid----------------------------------------------
SELECT RA.EmployeeId , CONCAT(U.FirstName,' ',U.LastName)AS ResourceName , 
 RU.RoleName AS Role,
 RA.ProjectId,
 PD.ProjectName,
 SUBSTRING(AL.AllocationDescription,0,Len(AL.AllocationDescription)) As Utilization,
 U.FormattedEmployeeId
FROM PMSNexus_Projects.[dbo].[ResourceAllocation] RA 
JOIN PMSNexus_Employees.[dbo].[Employees] U ON U.EmployeeId=RA.EmployeeId
Join PMSNexus_Projects.[dbo].[ProjectDetails] PD On Pd.ProjectId=RA.ProjectId
Join PMSNexus_Projects.[dbo].[Allocation] AL On RA.AllocationId=AL.AllocationId
JOIN PMSNexus_Employees.[dbo].[Roles] RU ON RU.RoleId=U.RoleId
Where RA.EmployeeId Is Not Null;

---------------------------Project Wise Resource Utilisation----------------------------

Select CAST(ROW_NUMBER() OVER(ORDER BY PD.ProjectName) AS INT) AS Id, PD.ProjectName,
(Select Count(DISTINCT(TL.ResourceId)) From PMSNexus_Timesheets.[dbo].[TimesheetLog] TL 
JOIN PMSNexus_Timesheets.[dbo].[Timesheet] T ON T.[TimesheetId]=TL.[TimesheetId]
where TL.ProjectId=PD.ProjectId And T.IsBillable=1) As Billable,
(Select Count(DISTINCT(TL.ResourceId)) From PMSNexus_Timesheets.[dbo].[TimesheetLog] TL 
JOIN PMSNexus_Timesheets.[dbo].[Timesheet] T ON T.[TimesheetId]=TL.[TimesheetId]
where TL.ProjectId=PD.ProjectId And T.IsBillable=0) As NonBillable
FROM PMSNexus_Projects.[dbo].[ProjectDetails] PD;
------------------------------Resource Billability Status-------------------------------
With Billable As
(Select distinct U.[EmployeeId] UserId,'Billable' as Status
 FROM PMSNexus_Employees.[dbo].[Employees] U 
 Join PMSNexus_Timesheets.[dbo].[TimesheetLog] TL ON U.EmployeeId=TL.ResourceID
 Join PMSNexus_Timesheets.[dbo].[Timesheet] T ON T.[TimesheetId]=TL.[TimesheetId]
Where T.IsBillable=1),
NonBillable As
(Select distinct U.EmployeeId UserId,'Non-Billable' as Status
 FROM PMSNexus_Employees.[dbo].[Employees] U 
 Join PMSNexus_Timesheets.[dbo].[TimesheetLog] TL ON U.EmployeeId=TL.ResourceID
 Join PMSNexus_Timesheets.[dbo].[Timesheet] T ON T.[TimesheetId]=TL.[TimesheetId]
Where T.IsBillable=0),
Bench As
(Select distinct U.EmployeeId UserId,'Bench' as Status
 FROM PMSNexus_Employees.[dbo].[Employees] U 
 Join PMSNexus_Timesheets.[dbo].[TimesheetLog] TL ON U.EmployeeId != TL.ResourceID
 ),
AllResource As 
(Select * from Billable Union All
 Select * from NonBillable Union All
 Select * from Bench )

 Select CAST(ROW_NUMBER() OVER(ORDER BY Al.Status) AS INT) AS Id, COUNT(Al.UserId) As Count,Al.Status from AllResource Al
 Group By Al.Status



-----------------------------------SkillSet Wise Resource--------------------------------------
	
SELECT CAST(ROW_NUMBER() OVER(ORDER BY RS.RequiredSkillSetDescription) AS INT) AS Id,
RS.[RequiredSkillSetDescription] AS Status/*SkillSet*/,
RA.ProjectId,
COUNT (DISTINCT RA.[EmployeeID]) AS [Count]/*Resource*/
FROM PMSNexus_Projects.[dbo].[RequiredSkillSet] RS
JOIN PMSNexus_Projects.[dbo].[ResourceAllocation]RA ON RA.[RequiredSkillSetId]=RS.[RequiredSkillSetId]
GROUP BY RS.[RequiredSkillSetDescription],RA.ProjectId

---------------------------------SkillSet Wise Bench--------------------------------------------

--No data

---------------------------------Resource Wise Utillisation--------------------------------------

SELECT CAST(ROW_NUMBER() OVER(ORDER BY U.[EmployeeId]) AS INT) AS Id, 
U.[EmployeeId] UserId, CONCAT(U.[FirstName],' ',U.[LastName])as Status,
RA.ProjectId,
Sum( Cast( SUBSTRING(AL.AllocationDescription,0,Len(AL.AllocationDescription))As Int)) As Count
From PMSNexus_Employees.[dbo].[Employees] U
JOIN PMSNexus_Projects.[dbo].[ResourceAllocation] RA ON RA.[EmployeeId]=U.[EmployeeId]
Join PMSNexus_Projects.[dbo].[Allocation] AL On RA.AllocationId=AL.AllocationId
GROUP BY U.[EmployeeId],U.FirstName,U.LastName,RA.ProjectId

---------------------------------Active Project List--------------------------------------

Select CAST(ROW_NUMBER() OVER(ORDER BY PD.ProjectId) AS INT) AS Id,PD.ProjectId as Count, PD.ProjectName as Status 
from PMSNexus_Projects.[dbo].[ProjectDetails] PD 
where PD.ProjectStatus='Ongoing'

 END TRY
     BEGIN CATCH
        EXEC [InsertSqlExceptionLog]
     END CATCH
END


--EXEC [GetResourceReport] 
