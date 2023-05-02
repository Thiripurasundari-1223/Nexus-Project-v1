USE [PMSNexus_Leaves]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'IsGrantRequestPastDay')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD IsGrantRequestPastDay bit null
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'GrantRequestPastDay')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD GrantRequestPastDay int null
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'IsGrantRequestFutureDay')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD IsGrantRequestFutureDay bit null
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'GrantRequestFutureDay')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD GrantRequestFutureDay int null
END
GO
