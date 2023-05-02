USE [PMSNexus_Leaves]
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'MinimumLeavePerApplication')
BEGIN
	EXEC sp_RENAME 'LeaveRestrictions.MinimumLeavePerApplication','MaximumLeavePerApplication','COLUMN'
END
GO
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'MinimumLeave')
BEGIN
	EXEC sp_RENAME 'LeaveRestrictions.MinimumLeave','MaximumLeave','COLUMN'
END
GO
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'MinimumConsecutiveDays')
BEGIN
	EXEC sp_RENAME 'LeaveRestrictions.MinimumConsecutiveDays','MaximumConsecutiveDays','COLUMN'
END
GO
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'MinimumConsecutive')
BEGIN
	EXEC sp_RENAME 'LeaveRestrictions.MinimumConsecutive','MaximumConsecutive','COLUMN'
END
GO




