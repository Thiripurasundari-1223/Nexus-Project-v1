USE [PMSNexus_Leaves]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'AllowPastDates')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD AllowPastDates bit null
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'AllowFutureDates')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD AllowFutureDates bit null
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'IsAllowRequestNextDays')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD IsAllowRequestNextDays bit null
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'IsToBeApplied')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD IsToBeApplied bit null
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'Weekendsbetweenleaveperiod')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD Weekendsbetweenleaveperiod bit null
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'Holidaybetweenleaveperiod')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD Holidaybetweenleaveperiod bit null
END
GO