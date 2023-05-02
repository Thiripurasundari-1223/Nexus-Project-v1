USE [PMSNexus_Attendance]

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='TimeDefinition')
BEGIN
	ALTER TABLE [dbo].[TimeDefinition] DROP COLUMN [BreakTime],[TotalHours],
	[AbsentFromHour],[AbsentToHour],[HalfaDayFromHour],[HalfaDayToHour],[PresentHour]
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='TimeDefinition' AND COLUMN_NAME = 'BreakTime' AND COLUMN_NAME = 'TotalHours' 
AND COLUMN_NAME = 'AbsentFromHour' AND COLUMN_NAME = 'AbsentToHour' AND COLUMN_NAME = 'HalfaDayFromHour' AND COLUMN_NAME = 'HalfaDayToHour' AND COLUMN_NAME = 'PresentHour')
BEGIN
	ALTER TABLE [dbo].[TimeDefinition] ADD BreakTime [Time](0) NULL
	ALTER TABLE [dbo].[TimeDefinition] ADD TotalHours [Time](0) NULL
	ALTER TABLE [dbo].[TimeDefinition] ADD AbsentFromHour [Time](0) NULL
	ALTER TABLE [dbo].[TimeDefinition] ADD AbsentToHour [Time](0) NULL
	ALTER TABLE [dbo].[TimeDefinition] ADD HalfaDayFromHour [Time](0) NULL
	ALTER TABLE [dbo].[TimeDefinition] ADD HalfaDayToHour [Time](0) NULL
	ALTER TABLE [dbo].[TimeDefinition] ADD PresentHour [Time](0) NULL
END
GO