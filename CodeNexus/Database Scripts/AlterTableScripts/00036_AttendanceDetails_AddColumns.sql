
USE PMSNexus_Attendance
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AttendanceDetail' AND COLUMN_NAME = 'Reason' AND COLUMN_NAME = 'Status')
alter table [dbo].[AttendanceDetail] add Reason Nvarchar(500)
alter table [dbo].[AttendanceDetail] add Status bit
GO