USE [PMSNexus_Leaves]
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Holiday' AND COLUMN_NAME = 'EmployeeGroupId')
BEGIN
	ALTER TABLE [dbo].[Holiday] DROP COLUMN [EmployeeGroupId],[EmployeeDepartmentId],[EmployeeRegionId]
END
GO