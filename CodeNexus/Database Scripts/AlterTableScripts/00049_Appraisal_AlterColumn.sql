USE [PMSNexus_Appraisal]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeGroupSelection' AND COLUMN_NAME = 'IsSelected')
BEGIN
	ALTER TABLE [dbo].[EmployeeGroupSelection] ADD IsSelected bit NULL
END
GO
