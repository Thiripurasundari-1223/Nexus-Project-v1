USE [PMSNexus_Attendance]

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AttendanceDetail' AND COLUMN_NAME = 'Isregularize' )
	BEGIN
	ALTER TABLE AttendanceDetail ADD Isregularize bit NULL
END
GO