USE [PMSNexus_Leaves]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'GrantMinimumNoOfRequestDay')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD GrantMinimumNoOfRequestDay int null
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'GrantMaximumNoOfRequestDay')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD GrantMaximumNoOfRequestDay int null
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'GrantMaximumNoOfPeriod')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD GrantMaximumNoOfPeriod int null
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'GrantMaximumNoOfDay')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD GrantMaximumNoOfDay int null
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'GrantMinimumGapTwoApplicationDay')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD GrantMinimumGapTwoApplicationDay int null
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'GrantUploadDocumentSpecificPeriodDay')
BEGIN
	ALTER TABLE [dbo].[LeaveRestrictions] ADD GrantUploadDocumentSpecificPeriodDay int null
END
GO