USE [PMSNexus_Projects]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ChangeRequest' AND COLUMN_NAME = 'CRStatusCode')
BEGIN
	ALTER TABLE [dbo].[ChangeRequest] ADD CRStatusCode [varchar](100) NULL
END
GO