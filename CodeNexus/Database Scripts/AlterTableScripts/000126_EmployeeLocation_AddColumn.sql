USE [PMSNexus_Employees]
GO

IF Not EXISTS  (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeLocation' AND COLUMN_NAME='ModifiedBy')
BEGIN
				ALTER TABLE [dbo].[EmployeeLocation] add [ModifiedBy] [int] NULL

END
IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeLocation' AND COLUMN_NAME='ModifiedOn')
BEGIN
           ALTER TABLE [dbo].[EmployeeLocation] add [ModifiedOn] [datetime] NULL

END