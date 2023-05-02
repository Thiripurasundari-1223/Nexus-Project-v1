USE [PMSNexus_Accounts]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AccountDetails' AND COLUMN_NAME = 'AccountChanges')
BEGIN
	ALTER TABLE [dbo].[AccountDetails] ADD AccountChanges [varchar](max) NULL
END
GO