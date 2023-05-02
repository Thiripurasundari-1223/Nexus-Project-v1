
USE [PMSNexus_Employees]
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeesDesignationHistory')
BEGIN
		ALTER TABLE [dbo].[EmployeesDesignationHistory] drop column ModifiedOn
		ALTER TABLE [dbo].[EmployeesDesignationHistory] drop column ModifiedBy
END
GO