USE [PMSNexus_Projects]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ProjectDetails' AND COLUMN_NAME = 'ProjectChanges')
BEGIN
	ALTER TABLE [dbo].[ProjectDetails] ADD ProjectChanges [varchar](max) NULL
END
GO