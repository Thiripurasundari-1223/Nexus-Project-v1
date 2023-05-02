USE [PMSNexus_Leaves]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'GrantResetLeaveAfterDays' AND COLUMN_NAME = 'ToBeAdvanced')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD [GrantResetLeaveAfterDays] [decimal](18, 2) NULL
	ALTER TABLE [dbo].[LeaveRestrictions] ADD [ToBeAdvanced] [decimal](18, 2) NULL
END
GO