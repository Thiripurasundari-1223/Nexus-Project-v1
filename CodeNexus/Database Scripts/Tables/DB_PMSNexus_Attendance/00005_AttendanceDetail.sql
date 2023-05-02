
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AttendanceDetail')
begin
create table AttendanceDetail
(
AttendanceDetailId [int] identity(1,1) primary key,
AttendanceId int,
CheckinTime datetime,
CheckoutTime datetime,
TotalHours Time(0),
BreakHours Time(0),
BreakoutTime datetime,
RejectReason varchar(500),
Reason Nvarchar(500),
Status bit,
Isregularize bit,
CreatedOn datetime NULL,
CreatedBy int NULL,
ManagerId int NULL
)
end