
USE [PMSNexus_Employees]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Employees' AND COLUMN_NAME = 'Gender' AND COLUMN_NAME = 'LocationId ' AND COLUMN_NAME = 'IsSpecialAbility')
BEGIN
	ALTER TABLE [dbo].[Employees] ADD Gender varchar(10) NULL
	ALTER TABLE [dbo].[Employees] ADD LocationId int NULL
	ALTER TABLE [dbo].[Employees] ADD IsSpecialAbility bit NULL
END
GO
