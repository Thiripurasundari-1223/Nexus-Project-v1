USE [PMSNexus_Leaves]
GO
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'GrantMinimumNoOfRequestDay')
BEGIN
	ALTER TABLE LeaveRestrictions ALTER COLUMN GrantMinimumNoOfRequestDay [decimal](18, 2) NULL
END
GO
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'GrantMaximumNoOfRequestDay')
BEGIN
	ALTER TABLE LeaveRestrictions ALTER COLUMN GrantMaximumNoOfRequestDay [decimal](18, 2) NULL
END
GO
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions'AND COLUMN_NAME = 'GrantMinimumGapTwoApplicationDay')
BEGIN
	ALTER TABLE LeaveRestrictions ALTER COLUMN GrantMinimumGapTwoApplicationDay [decimal](18, 2) NULL
END
GO
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'GrantUploadDocumentSpecificPeriodDay')
BEGIN
	ALTER TABLE LeaveRestrictions ALTER COLUMN GrantUploadDocumentSpecificPeriodDay [decimal](18, 2) NULL
END