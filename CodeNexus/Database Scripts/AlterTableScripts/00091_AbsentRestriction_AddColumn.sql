USE [PMSNexus_Attendance]
GO
ALTER TABLE AbsentRestrictions DROP COLUMN WeekendDontCountAsAbsent
ALTER TABLE AbsentRestrictions DROP COLUMN HolidayDontCountAsAbsent 
ALTER TABLE AbsentRestrictions DROP COLUMN WeekendCountAsAbsent
ALTER TABLE AbsentRestrictions DROP COLUMN HolidayCountAsAbsent 
GO

USE [PMSNexus_Attendance]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AbsentRestrictions' AND COLUMN_NAME = 'HolidaysBetweenAttendancePeriod' AND COLUMN_NAME = 'WeekendsBetweenAttendacePeriod')
BEGIN
	ALTER TABLE [dbo].[AbsentRestrictions] ADD [WeekendsBetweenAttendacePeriod] [bit] NULL
	ALTER TABLE [dbo].[AbsentRestrictions] ADD [HolidaysBetweenAttendancePeriod] [bit] NULL
END
GO