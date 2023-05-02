-- =============================================
-- Author:  <vindhya>
-- Create date: <2020 09 18>
-- Description: <Get Timesheet Report>
-- =============================================
IF (OBJECT_ID('GetTimesheetReport') IS NOT NULL)
  DROP PROCEDURE [GetTimesheetReport]
GO

CREATE PROCEDURE [GetTimesheetReport]
@ResourseId Int=NULL
AS
BEGIN    
BEGIN TRY
-----------------------------------------Timesheet Status--------------------------------------------
WITH Submitted AS
    (SELECT PD.[ProjectName],Count(Distinct(TL.[ResourceId])) AS Submitted
    FROM PMSNexus_Projects.[dbo].[ProjectDetails] PD
    JOIN PMSNexus_Timesheets.[dbo].[TimesheetLog] TL on TL.[ProjectId]=PD.[ProjectId]
	WHERE TL.[IsSubmitted]=1
	GROUP BY PD.[ProjectName]),
NotSubmitted AS
	(SELECT PD.[ProjectName],Count(Distinct(TL.[ResourceId])) AS NotSubmitted
    FROM PMSNexus_Projects.[dbo].[ProjectDetails] PD
    JOIN PMSNexus_Timesheets.[dbo].[TimesheetLog] TL on TL.[ProjectId]=PD.[ProjectId]
	WHERE TL.[IsSubmitted]=0 AND TL.IsApproved=0 
	GROUP BY PD.[ProjectName])
	SELECT CAST(ROW_NUMBER() OVER(ORDER BY A.ProjectName) AS INT) AS Id, A.[ProjectName], A.[Submitted],	B.[NotSubmitted] 
	FROM Submitted A FULL OUTER JOIN 
	NotSubmitted B ON A.[ProjectName]=B.[ProjectName];

----------------------------------Resource Wise Weekly Timesheet Status-------------------------------

WITH Submitted AS
    (SELECT CONCAT(U.[FirstName],' ',U.[LastName]) AS ResourceName,SUM(TL.RequiredHours) AS PlannedHours,
    SUM(TL.[ClockedHours]) AS SubmittedHours 
    FROM PMSNexus_Employees.[dbo].[Employees] U
    JOIN PMSNexus_Timesheets.[dbo].[TimesheetLog] TL ON TL.[ResourceId]=U.EmployeeId
    WHERE TL.[IsSubmitted]=1
    GROUP BY U.[FirstName],U.[LastName]),
NotSubmitted AS
	(SELECT CONCAT(U.[FirstName],' ',U.[LastName]) AS ResourceName,SUM(TL.RequiredHours) AS PlannedHours,
    SUM(TL.[ClockedHours]) AS NotSubmittedHours 
    FROM PMSNexus_Employees.[dbo].[Employees] U
    JOIN PMSNexus_Timesheets.[dbo].[TimesheetLog] TL ON TL.[ResourceId]=U.EmployeeId
	WHERE TL.[IsSubmitted]=0
	GROUP BY U.[FirstName],U.[LastName])
	SELECT CAST(ROW_NUMBER() OVER(ORDER BY A.ResourceName) AS INT) AS Id, A.[ResourceName], A.[SubmittedHours],
	B.[NotSubmittedHours] ,B.[PlannedHours]
	FROM Submitted A FULL OUTER JOIN 
	NotSubmitted B ON A.[ResourceName]=B.[ResourceName]

------------------------------------Project Wise Timesheet Deficit-------------------------------------

SELECT CAST(ROW_NUMBER() OVER(ORDER BY PD.[ProjectName]) AS INT) AS Id,
PD.[ProjectName] AS ProjectName,SUM(TL.RequiredHours)AS PlannedHours,
SUM(TL.[ClockedHours]) As ActualHours
FROM PMSNexus_Projects.[dbo].[ProjectDetails] PD
JOIN PMSNexus_Timesheets.[dbo].[TimesheetLog] TL ON TL.[ProjectId]=PD.[ProjectId]
--Where TL.IsApproved=1
GROUP BY PD.[ProjectName]


-----------------------------------Planned Vs Actual Utilisation--------------------------------------
	
SELECT CAST(ROW_NUMBER() OVER(ORDER BY CONCAT(U.[FirstName],' ',U.[LastName])) AS INT) AS Id,
CONCAT(U.[FirstName],' ',U.[LastName]) AS ResourceName,SUM(TL.RequiredHours) AS PlannedHours,
SUM(TL.[ClockedHours]) As ActualHours
FROM PMSNexus_Employees.[dbo].[Employees] U
JOIN PMSNexus_Timesheets.[dbo].[TimesheetLog] TL ON TL.[ResourceId]=U.EmployeeId
GROUP BY U.[FirstName],U.[LastName]


-----------------------------------------grid---------------------------------------------------
SELECT * FROM (
SELECT PD.[ProjectId], PD.[ProjectName],TL.[ResourceId],CONCAT(U.[FirstName],' ' , U.[LastName]) AS ResourceName,
SUM(TL.[RequiredHours])AS PlannedHours,SUM(TL.[ClockedHours]) AS ClockedHours,CONVERT(DATE,TL.[PeriodSelection]) as DATEE
FROM PMSNexus_Projects.[dbo].[ProjectDetails] PD
JOIN PMSNexus_Timesheets.[dbo].[TimesheetLog] TL ON TL.[ProjectId]=PD.[ProjectId]
JOIN PMSNexus_Employees.[dbo].[Employees] U ON U.EmployeeId=TL.[ResourceId]
GROUP BY PD.[ProjectId], PD.[ProjectName],TL.[ResourceId],U.[FirstName],U.[LastName]
,TL.[PeriodSelection])AS A 
PIVOT (SUM(ClockedHours)
FOR DATEE IN ([2020-11-01], [2020-11-02],[2020-11-03],[2020-11-04],[2020-11-05]))
as PVT



	END TRY 
    BEGIN CATCH
        EXEC [InsertSqlExceptionLog]
    END CATCH
END

--EXEC GetTimesheetReport