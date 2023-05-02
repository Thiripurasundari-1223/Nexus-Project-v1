  
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Attendance')
begin
create table Attendance
(
AttendanceId int identity(1,1) primary key,
EmployeeId int not null,
Date date not null,
TotalHours Time(0),
BreakHours Time(0),
IsCheckin bit,
CreatedOn datetime NULL,
CreatedBy int NULL,
ModifiedOn datetime NULL,
ModifiedBy int NULL
)
end