USE [PMSNexus_Employees]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Reports' AND COLUMN_NAME = 'ReportTitle' AND COLUMN_NAME = 'ReportIconPath ' AND COLUMN_NAME = 'ReportNavigationUrl')
BEGIN
	ALTER TABLE [dbo].[Reports] ADD ReportTitle varchar(100) NULL
	ALTER TABLE [dbo].[Reports] ADD ReportIconPath varchar(250) NULL
	ALTER TABLE [dbo].[Reports] ADD ReportNavigationUrl varchar(250) NULL
END
GO
