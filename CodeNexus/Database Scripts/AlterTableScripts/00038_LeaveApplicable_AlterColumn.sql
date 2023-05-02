USE [PMSNexus_Leaves]
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveApplicable')
BEGIN
	ALTER TABLE [dbo].[LeaveApplicable] DROP COLUMN [ApplicableDepartmentId],[ApplicableDesginationId],[ApplicableLocationId]
	,[ApplicableRoleId],[ExceptionsDepartmentId],[ExceptionsDesginationId],[ExceptionsLocationId],[ExceptionsRoleId]
END
GO