
USE [PMSNexus_Leaves]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveEmployeeType' AND COLUMN_NAME = 'LeaveExceptionEmployeeTypeId')
BEGIN
	ALTER TABLE [dbo].[LeaveEmployeeType] ADD LeaveExceptionEmployeeTypeId int NULL
END
GO

USE [PMSNexus_Leaves]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveProbationStatus' AND COLUMN_NAME = 'LeaveExceptionProbationStatus')
BEGIN
	ALTER TABLE [dbo].[LeaveProbationStatus] ADD LeaveExceptionProbationStatus int NULL
END
GO

USE [PMSNexus_Leaves]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeApplicableLeave' AND COLUMN_NAME = 'LeaveExceptionEmployeeId')
BEGIN
	ALTER TABLE [dbo].[EmployeeApplicableLeave] ADD LeaveExceptionEmployeeId int NULL
END
GO

USE [PMSNexus_Leaves]
BEGIN
EXEC sp_rename '[dbo].[LeaveEmployeeType].EmployeeTypeId' , 'LeaveApplicableEmployeeTypeId', 'COLUMN'
END
GO

USE [PMSNexus_Leaves]
BEGIN
EXEC sp_rename '[dbo].[LeaveProbationStatus].ProbationStatus' , 'LeaveApplicableProbationStatus', 'COLUMN'
END
GO

USE [PMSNexus_Leaves]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveApplicable' AND COLUMN_NAME = 'Gender_Male_Exception'
AND COLUMN_NAME = 'Gender_Female_Exception' AND COLUMN_NAME = 'Gender_Others_Exception' AND COLUMN_NAME = 'MaritalStatus_Single_Exception'
AND COLUMN_NAME = 'MaritalStatus_Married_Exception' AND COLUMN_NAME = 'Type')
BEGIN
	ALTER TABLE [dbo].[LeaveApplicable] ADD Gender_Male_Exception [bit] NULL
    ALTER TABLE [dbo].[LeaveApplicable] ADD Gender_Female_Exception [bit] NULL
	ALTER TABLE [dbo].[LeaveApplicable] ADD Gender_Others_Exception [bit] NULL
    ALTER TABLE [dbo].[LeaveApplicable] ADD MaritalStatus_Single_Exception [bit] NULL
	ALTER TABLE [dbo].[LeaveApplicable] ADD MaritalStatus_Married_Exception [bit] NULL
	ALTER TABLE [dbo].[LeaveApplicable] ADD Type nvarchar (50) NULL
END
GO