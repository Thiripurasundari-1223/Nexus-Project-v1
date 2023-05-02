USE [PMSNexus_Leaves]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveGrantRequestDetails' AND COLUMN_NAME = 'BalanceDay' )
BEGIN
	ALTER TABLE [dbo].[LeaveGrantRequestDetails] ADD [BalanceDay] [decimal](18, 2) NULL
END
GO