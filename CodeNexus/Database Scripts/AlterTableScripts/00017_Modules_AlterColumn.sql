USE [PMSNexus_Employees]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Modules' AND COLUMN_NAME = 'ModuleShortDescription' AND COLUMN_NAME = 'ModuleFullDescription' AND COLUMN_NAME = 'IsActive')
BEGIN
	ALTER TABLE [dbo].Modules ADD ModuleShortDescription nvarchar(2000) NULL
	ALTER TABLE [dbo].Modules ADD ModuleFullDescription nvarchar (Max) NULL
    ALTER TABLE [dbo].Modules ADD IsActive bit
END
GO