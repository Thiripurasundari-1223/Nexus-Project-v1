USE [PMSNexus_Leaves]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AppliedLeaveDetails' AND COLUMN_NAME = 'AppliedLeaveStatus')
BEGIN
	ALTER TABLE [dbo].[AppliedLeaveDetails] ADD AppliedLeaveStatus bit NULL
END
GO
