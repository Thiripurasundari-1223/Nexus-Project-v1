USE [PMSNexus_Attendance]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='TimeDefinition' AND COLUMN_NAME = 'IsConsiderAbsent' AND COLUMN_NAME = 'IsConsiderPresent' AND COLUMN_NAME = 'IsConsiderHalfaDay')
BEGIN
	ALTER TABLE [dbo].[TimeDefinition] ADD IsConsiderAbsent bit NULL
	ALTER TABLE [dbo].[TimeDefinition] ADD IsConsiderPresent bit NULL
	ALTER TABLE [dbo].[TimeDefinition] ADD IsConsiderHalfaDay bit NULL
END
GO
