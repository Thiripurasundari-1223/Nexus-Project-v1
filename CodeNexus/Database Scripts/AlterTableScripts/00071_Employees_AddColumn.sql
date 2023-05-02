USE [PMSNexus_Employees]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Employees' AND COLUMN_NAME = 'FormattedEmployeeId')
BEGIN
	ALTER TABLE [dbo].[Employees] ADD FormattedEmployeeId VARCHAR(100) NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Employees' AND COLUMN_NAME = 'EmployeeName')
BEGIN
	ALTER TABLE [dbo].[Employees] ADD EmployeeName VARCHAR(1000) NULL
END
GO