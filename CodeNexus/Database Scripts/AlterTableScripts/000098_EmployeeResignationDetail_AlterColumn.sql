USE [PMSNexus_ExitManagement]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'AddressLine2'
 AND COLUMN_NAME = 'City'
 AND COLUMN_NAME = 'ZipCode'
 AND COLUMN_NAME = 'StateId'
 AND COLUMN_NAME = 'CountryId'
 AND COLUMN_NAME = 'ResignationDate'
 AND COLUMN_NAME = 'RelievingDate'
 AND COLUMN_NAME = 'WithdrawalReason'
 AND COLUMN_NAME = 'ResignationReason'
 AND COLUMN_NAME = 'ReportingManagerId'
 And COLUMN_NAME = 'DepartmentID'
 AND COLUMN_NAME = 'LocationId')
BEGIN
	ALTER TABLE [dbo].[EmployeeResignationDetails] ADD AddressLine2 [nvarchar](max) NULL
	ALTER TABLE [dbo].[EmployeeResignationDetails] ADD City [nvarchar](100) NULL
	ALTER TABLE [dbo].[EmployeeResignationDetails] ADD ZipCode [nvarchar](100) NULL
	ALTER TABLE [dbo].[EmployeeResignationDetails] ADD StateId int NULL
	ALTER TABLE [dbo].[EmployeeResignationDetails] ADD CountryId int NULL
	ALTER TABLE [dbo].[EmployeeResignationDetails] ADD ResignationDate [datetime] NULL
	ALTER TABLE [dbo].[EmployeeResignationDetails] ADD RelievingDate [datetime] NULL
	ALTER TABLE [dbo].[EmployeeResignationDetails] ADD WithdrawalReason [nvarchar](max) NULL
	ALTER TABLE [dbo].[EmployeeResignationDetails] ADD ResignationReason [nvarchar](max) NULL
	ALTER TABLE [dbo].[EmployeeResignationDetails] ADD ReportingManagerId int NULL
	ALTER TABLE [dbo].[EmployeeResignationDetails] ADD DepartmentID int NULL
	ALTER TABLE [dbo].[EmployeeResignationDetails] ADD LocationId int NULL
END
GO