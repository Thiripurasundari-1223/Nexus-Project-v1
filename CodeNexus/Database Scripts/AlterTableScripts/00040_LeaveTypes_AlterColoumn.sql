USE [PMSNexus_Leaves]

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveTypes')
BEGIN
	ALTER TABLE [dbo].[LeaveTypes] DROP COLUMN [LeaveAccruedMonthlyId],[LeaveAccruedLastDayId]
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveTypes' AND COLUMN_NAME = 'LeaveAccruedMonthly' AND COLUMN_NAME = 'LeaveAccruedLastDay')
BEGIN
	ALTER TABLE [dbo].[LeaveTypes] ADD LeaveAccruedMonthly [nvarchar](100) NULL
	ALTER TABLE [dbo].[LeaveTypes] ADD LeaveAccruedLastDay [nvarchar](100) NULL
END
GO