use [PMSNexus_Attendance]
/*

1)Delete already mapped shift

truncate table [dbo].[AttendanceDetail]
truncate table [dbo].[Attendance]
truncate table [dbo].[ShiftDetails]
truncate table [dbo].[TimeDefinition]
truncate table [dbo].[ShiftWeekendDefinition]

truncate table [dbo].[AbsentDepartment]
truncate table [dbo].[AbsentDesignation]
truncate table [dbo].[AbsentEmployee]
truncate table [dbo].[AbsentEmployeeType]
truncate table [dbo].[AbsentLocation]
truncate table [dbo].[AbsentProbationStatus]
truncate table [dbo].[AbsentRestrictions]
truncate table [dbo].[AbsentRole]
truncate table [dbo].[AbsentSetting]
GO
TRUNCATE TABLE [dbo].[ShiftDetails]
GO
SET IDENTITY_INSERT [dbo].[ShiftDetails] ON
GO
INSERT INTO [dbo].[ShiftDetails] ([ShiftDetailsId],[ShiftName],[ShiftCode],[TimeFrom],[TimeTo],[ShiftDescription],[EmployeeGroupId],[OverTime],[CreatedOn],[ModifiedOn],[CreatedBy],[ModifiedBy],[IsActive])
VALUES(1,'General Shift','GN','10:00:00','19:00:00','',0,0,GETDATE(),GETDATE(),1,1,1)
GO
TRUNCATE TABLE [dbo].[TimeDefinition]
GO
INSERT INTO [dbo].[TimeDefinition] ([TimeFrom],[TimeTo],[ShiftDetailsId],[CreatedOn],[ModifiedOn],[CreatedBy],[ModifiedBy],[IsConsiderAbsent],[IsConsiderPresent],[IsConsiderHalfaDay],[AbsentFromOperator],[AbsentToOperator],[HalfaDayFromOperator],[HalfaDayToOperator],[PresentOperator],[BreakTime],[TotalHours],[AbsentFromHour],[AbsentToHour],[HalfaDayFromHour],[HalfaDayToHour],[PresentHour])
VALUES('00:00:00','00:00:00',1,GETDATE(),GETDATE(),1,1,1,1,1,'greaterThan','lessThan','greaterThanEqualTo','lessThan','greaterThanEqualTo','00:00:00','08:00:00','00:00:00','04:00:00','04:00:00','08:00:00','08:00:00')
GO
TRUNCATE TABLE [dbo].[ShiftWeekendDefinition]
GO
INSERT INTO [dbo].[ShiftWeekendDefinition] ([WeekendDayId],[ShiftDetailsId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy])
VALUES(1,1,GETDATE(),1,GETDATE(),1)
INSERT INTO [dbo].[ShiftWeekendDefinition] ([WeekendDayId],[ShiftDetailsId],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy])
VALUES(7,1,GETDATE(),1,GETDATE(),1)
GO



*/

