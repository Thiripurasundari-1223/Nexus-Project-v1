USE [PMSNexus_Employees]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Department' AND COLUMN_NAME = 'ParentDepartmentId' AND COLUMN_NAME = 'DepartmentHeadNameEmployeeId' )
BEGIN
	ALTER TABLE [dbo].Department ADD ParentDepartmentId [int] NULL
	ALTER TABLE [dbo].Department ADD DepartmentHeadEmployeeId [int] NULL
   
END
GO