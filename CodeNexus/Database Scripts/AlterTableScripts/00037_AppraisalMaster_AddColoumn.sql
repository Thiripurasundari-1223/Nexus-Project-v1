USE [PMSNexus_Appraisal]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AppraisalMaster' AND COLUMN_NAME = 'DateOfJoining' AND COLUMN_NAME = 'EmployeeTypeId')
BEGIN
	ALTER TABLE [dbo].[AppraisalMaster] ADD DateOfJoining Date NULL
	ALTER TABLE [dbo].[AppraisalMaster] ADD EmployeesTypeId int NULL
END
GO
