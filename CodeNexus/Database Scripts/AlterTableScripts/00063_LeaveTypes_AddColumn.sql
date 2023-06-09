USE [PMSNexus_Leaves]
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveTypes' AND COLUMN_NAME = 'IsActive')
BEGIN
	ALTER TABLE LeaveTypes DROP COLUMN IsActive
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveTypes' AND COLUMN_NAME = 'ProRate' AND COLUMN_NAME = 'EffectiveFromDate' AND COLUMN_NAME = 'EffectiveToDate')
BEGIN
    ALTER TABLE [dbo].[LeaveTypes] ADD ProRate int NULL
	ALTER TABLE [dbo].[LeaveTypes] ADD EffectiveFromDate datetime NULL
	ALTER TABLE [dbo].[LeaveTypes] ADD EffectiveToDate datetime NULL
END
GO