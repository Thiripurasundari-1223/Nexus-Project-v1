USE [PMSNexus_Accounts]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AccountDetails' AND COLUMN_NAME = 'AccountStatusCode')
BEGIN
	ALTER TABLE [dbo].[AccountDetails] ADD AccountStatusCode [varchar](100) NULL
END
GO