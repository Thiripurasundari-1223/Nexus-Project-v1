USE [PMSNexus_Leaves]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveGrantRequestDetails' AND COLUMN_NAME = 'IsLeaveAdjustment' )
BEGIN
	ALTER TABLE [dbo].[LeaveGrantRequestDetails] ADD [IsLeaveAdjustment] [bit] NULL
END
GO