USE [PMSNexus_Projects]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ProjectDetails' AND COLUMN_NAME = 'ProjectStatusCode')
BEGIN
	ALTER TABLE [dbo].[ProjectDetails] ADD ProjectStatusCode [varchar](100) NULL
END
GO