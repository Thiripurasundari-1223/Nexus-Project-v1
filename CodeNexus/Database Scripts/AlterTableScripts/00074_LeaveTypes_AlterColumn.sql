USE [PMSNexus_Leaves]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveTypes' AND COLUMN_NAME = 'BalanceBasedOn')
BEGIN
	ALTER TABLE [dbo].[LeaveTypes] ADD BalanceBasedOn int NULL
END
GO