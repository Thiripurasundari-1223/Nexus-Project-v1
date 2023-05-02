
USE [PMSNexus_Employees]
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Employees' AND COLUMN_NAME='SpecialAbilityId')
BEGIN
		ALTER TABLE [dbo].[Employees] drop column [SpecialAbilityId]
END

