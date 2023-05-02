USE PMSNexus_Attendance
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AttendanceDetail' AND COLUMN_NAME = 'BreakoutTime')
	ALTER TABLE AttendanceDetail ADD BreakoutTime datetime
GO

