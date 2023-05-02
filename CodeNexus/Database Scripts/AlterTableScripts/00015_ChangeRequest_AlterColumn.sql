
USE [PMSNexus_Projects]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ChangeRequest' AND COLUMN_NAME = 'FormattedChangeRequestId')
BEGIN
	ALTER TABLE [dbo].ChangeRequest ADD FormattedChangeRequestId varchar(250) NULL
END
GO