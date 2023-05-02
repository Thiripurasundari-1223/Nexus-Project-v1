USE [PMSNexus_Accounts]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AccountDetails' AND COLUMN_NAME = 'FormattedAccountId')
BEGIN
	ALTER TABLE [dbo].AccountDetails ADD FormattedAccountId varchar(250) NULL
END
GO
